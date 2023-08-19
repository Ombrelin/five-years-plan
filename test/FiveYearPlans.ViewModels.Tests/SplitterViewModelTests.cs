using FiveYearPlans.ViewModels.Buildings.ViewModels;
using FiveYearPlans.ViewModels.Tests.Fakes;
using NSubstitute;

namespace FiveYearPlans.ViewModels.Tests;

public class SplitterViewModelTests
{
    private readonly SplitterViewModel target = new();

    [Fact]
    public void RecomputeResourceFlowsFromIO_MoreThanOneInputThrows()
    {
        // Given
        target.InputResourceFlows[0] = new ResourceFlow(new Resource("Iron Ore"), 30);
        target.InputResourceFlows[1] = new ResourceFlow(new Resource("Iron Ore"), 30);

        // When
        var act = () => target.RecomputeOutput(Substitute.For<IBuildingContextProvider>());

        // Then
        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void RecomputeResourceFlowsFromIO_NoOutputFlowConnected_AllOutputsAtZero()
    {
        // Given
        var miner = new MinerViewModel();
        var fakeBuildingContext = new FakeBuildingContextProvider
        {
            Buildings =
            {
                [target.Id] = new Dictionary<uint, DynamicFlowBuilding?>
                {
                    [0] = null,
                    [1] = null,
                    [2] = null
                }
            }
        };


        // When
        new BuildingConnector(fakeBuildingContext).ConnectBuildings(0,0, target, miner);


        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 30), target.InputResourceFlows[0]);
        
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlow3);
    }
    
    [Fact]
    public EndBuilding OneOutputFlowConnected_FullOutputFlowFromInputOnIt()
    {
        // Given
        
        var endBuilding = new EndBuilding();
        var fakeBuildingContext = new FakeBuildingContextProvider
        {
            Buildings =
            {
                [target.Id] = new Dictionary<uint, DynamicFlowBuilding?>
                {
                    [0] = endBuilding,
                    [1] = null,
                    [2] = null
                }
            }
        };

        RecomputeResourceFlowsFromIO_NoOutputFlowConnected_AllOutputsAtZero();


        // When
        new BuildingConnector(fakeBuildingContext).ConnectBuildings(0,0, endBuilding, target);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 30), target.InputResourceFlows[0]);

        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 30), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 30), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlow3);
        
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 30), endBuilding.RecomputedResourceFlow);

        return endBuilding;
    }
    
    
    [Fact]
    public void RecomputeResourceFlowsFromIO_TwoOutputFlowConnectedCase1_SplitInputOnEach()
    {
        // Given
        var previousEndBuilding = OneOutputFlowConnected_FullOutputFlowFromInputOnIt();

        var endBuilding = new EndBuilding();
        var fakeBuildingContext = new FakeBuildingContextProvider
        {
            Buildings =
            {
                [target.Id] = new Dictionary<uint, DynamicFlowBuilding?>
                {
                    [0] = previousEndBuilding,
                    [1] = null,
                    [2] = endBuilding
                }
            }
        };

        // When
        new BuildingConnector(fakeBuildingContext).ConnectBuildings(0,0, endBuilding, target);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), target.OutPutResourceFlow3);
        
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), previousEndBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), endBuilding.RecomputedResourceFlow);

    }
    
    /*
    [Fact]
    public void RecomputeResourceFlowsFromIO_TwoOutputFlowConnectedCase2_SplitInputOnEach()
    {
        // Given
        target.InputResourceFlows[0] = new ResourceFlow(new Resource("Iron Ore"), 30);


        // When
        target.RecomputeOutput(new Dictionary<uint, bool>
        {
            [0] = false,
            [1] = true,
            [2] = true
        });

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), target.OutPutResourceFlow3);
    }
    
    [Fact]
    public void RecomputeResourceFlowsFromIO_TwoOutputFlowConnectedCase3_SplitInputOnEach()
    {
        // Given
        target.InputResourceFlows[0] = new ResourceFlow(new Resource("Iron Ore"), 30);


        // When
        target.RecomputeOutput(new Dictionary<uint, bool>
        {
            [0] = true,
            [1] = true,
            [2] = false
        });

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlow3);
    }

    
    [Fact]
    public void RecomputeResourceFlowsFromIO_ThreeOutputFlowConnected_SplitInputOnEach()
    {
        // Given
        target.InputResourceFlows[0] = new ResourceFlow(new Resource("Iron Ore"), 30);


        // When
        target.RecomputeOutput(new Dictionary<uint, bool>
        {
            [0] = true,
            [1] = true,
            [2] = true
        });

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlow3);
    }
    */
}