using System.Collections.ObjectModel;
using FiveYearPlans.ViewModels.Buildings;
using FiveYearPlans.ViewModels.Buildings.ViewModels;
using FiveYearPlans.ViewModels.Recipes;
using FiveYearPlans.ViewModels.Tests.Fakes;
using NSubstitute;

namespace FiveYearPlans.ViewModels.Tests;

public class MergerViewModelTests
{
    private readonly MergerViewModel target = new();

    [Fact]
    public void RecomputeResourceFlowsFromIO_MoreThanOneOutputThrows()
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
    public void Connect_NoOutputFlowConnected_OuputAndOtherInputsAtZero()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();

        // When
        ConnectMinerToTarget(fakeBuildingContext, 0);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 30), target.InputResourceFlows[0]);
        Assert.Null(target.InputResourceFlows[1]);
        Assert.Null(target.InputResourceFlows[2]);

        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlows[0]);
    }

    [Fact]
    public void Connect_OneOutputFlowConnected_FullOutputFlowFromInputOnIt()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();
        ConnectMinerToTarget(fakeBuildingContext, 0);

        // When
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 30), target.InputResourceFlows[0]);
        Assert.Null(target.InputResourceFlows[1]);
        Assert.Null(target.InputResourceFlows[2]);
        
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 30), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 30), endBuilding.RecomputedResourceFlow);
    }


    [Fact]
    public void Connect_TwoInputFlowConnectedCase1_MergeInputOnOutput()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();

        ConnectMinerToTarget(fakeBuildingContext, 0);
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);
        

        // When
        ConnectMinerToTarget(fakeBuildingContext, 1);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 60), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 60), endBuilding.RecomputedResourceFlow);
    }


    [Fact]
    public void Connect_TwoInputFlowConnectedCase2_MergeInputOnOutput()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();

        ConnectMinerToTarget(fakeBuildingContext, 0);
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);

        // When
        ConnectMinerToTarget(fakeBuildingContext, 2);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 60), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 60), endBuilding.RecomputedResourceFlow);
    }

    [Fact]
    public void Connect_TwoInputFlowConnectedCase3_MergeInputOnOutput()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();

        ConnectMinerToTarget(fakeBuildingContext, 1);
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);

        // When
        ConnectMinerToTarget(fakeBuildingContext, 2);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 60), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 60), endBuilding.RecomputedResourceFlow);
    }


    [Fact]
    public void Connect_ThreeOutputFlowConnectedOrder1_MergeInputOnOutput()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();
        ConnectMinerToTarget(fakeBuildingContext, 0);
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);
        ConnectMinerToTarget(fakeBuildingContext, 1);
        
        // When
        ConnectMinerToTarget(fakeBuildingContext, 2);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 90), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 90), endBuilding.RecomputedResourceFlow);
    }

    [Fact]
    public void Connect_ThreeOutputFlowConnectedOrder2_MergeInputOnOutput()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();
        ConnectMinerToTarget(fakeBuildingContext, 1);
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);
        ConnectMinerToTarget(fakeBuildingContext, 0);
        
        // When
        ConnectMinerToTarget(fakeBuildingContext, 2);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 90), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 90), endBuilding.RecomputedResourceFlow);
    }

    [Fact]
    public void Connect_ThreeOutputFlowConnectedOrder3_MergeInputOnOutput()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();
        ConnectMinerToTarget(fakeBuildingContext, 2);
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);
        ConnectMinerToTarget(fakeBuildingContext, 1);
        
        // When
        ConnectMinerToTarget(fakeBuildingContext, 0);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 90), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 90), endBuilding.RecomputedResourceFlow);
    }

    [Fact]
    public void Connect_ThreeOutputFlowConnectedOrder4_MergeInputOnOutput()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();
        ConnectMinerToTarget(fakeBuildingContext, 2);
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);
        ConnectMinerToTarget(fakeBuildingContext, 0);
        
        // When
        ConnectMinerToTarget(fakeBuildingContext, 1);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 90), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 90), endBuilding.RecomputedResourceFlow);
    }

    [Fact]
    public void Connect_ThreeOutputFlowConnectedOrder5_MergeInputOnOutput()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();
        ConnectMinerToTarget(fakeBuildingContext, 0);
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);
        ConnectMinerToTarget(fakeBuildingContext, 2);
        
        // When
        ConnectMinerToTarget(fakeBuildingContext, 1);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 90), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 90), endBuilding.RecomputedResourceFlow);
    }


    [Fact]
    public void Connect_ThreeOutputFlowConnectedOrder6_MergeInputOnOutput()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();
        ConnectMinerToTarget(fakeBuildingContext, 1);
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);
        ConnectMinerToTarget(fakeBuildingContext, 2);
        
        // When
        ConnectMinerToTarget(fakeBuildingContext, 0);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 90), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 90), endBuilding.RecomputedResourceFlow);
    }

    [Fact]
    public void Disconnect_ThreeOutputFlowConnectedRestoreToTwo_Merge()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();
        var miner = ConnectMinerToTarget(fakeBuildingContext, 0);
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);
        ConnectMinerToTarget(fakeBuildingContext, 1);
        ConnectMinerToTarget(fakeBuildingContext, 2);

        // When
        DisconnectMinerFromTarget(fakeBuildingContext, miner, 0);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 60), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 60), endBuilding.RecomputedResourceFlow);
    }

    [Fact]
    public void Disconnect_TwoOutputFlowConnectedRestoreToOne_Merge()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();

        ConnectMinerToTarget(fakeBuildingContext, 1);
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);
        var miner = ConnectMinerToTarget(fakeBuildingContext, 2);

        // When
        DisconnectMinerFromTarget(fakeBuildingContext, miner, 2);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 30), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 30), endBuilding.RecomputedResourceFlow);
    }

    [Fact]
    public void Disconnect_OneOutputFlowConnectedRestoreToZeroOutput_SplitInputOnEach()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();
        var miner = ConnectMinerToTarget(fakeBuildingContext, 1);
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);

        // When
        DisconnectMinerFromTarget(fakeBuildingContext, miner, 1);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), endBuilding.RecomputedResourceFlow);
    }


    private FakeBuildingContextProvider FakeBuildingContextProviderWithTarget() =>
        new()
        {
            Buildings =
            {
                [target.Id] = new Dictionary<uint, Building?>
                {
                    [0] = null,
                    [1] = null,
                    [2] = null
                }
            }
        };

    private MinerViewModel ConnectMinerToTarget(FakeBuildingContextProvider fakeBuildingContextProvider, uint inputNumber)
    {
        MinerViewModel miner = BuildIronOreMiner();
        fakeBuildingContextProvider.Buildings[miner.Id] = new Dictionary<uint, Building?>
        {
            [inputNumber] = target
        };
        new BuildingConnector(fakeBuildingContextProvider).ConnectBuildings(0, inputNumber, target, miner);

        return miner;
    }

    public MinerViewModel BuildIronOreMiner()
    {
        return new MinerViewModel()
        {
            PossibleRecipes = new ObservableCollection<Recipe>
            {
                new Recipe(
                    "Iron Ore",
                    Array.Empty<ResourceFlow>(),
                    new[]
                    {
                        new ResourceFlow(
                            new Resource("Iron Ore"),
                            30
                        )
                    })
            }
        };
    }


    private EndBuilding ConnectEndBuildingToTarget(FakeBuildingContextProvider fakeBuildingContext)
    {
        const uint outputIndex = 0;
        var endBuilding = new EndBuilding();
        fakeBuildingContext.Buildings[endBuilding.Id] = new Dictionary<uint, Building?>();
        fakeBuildingContext.Buildings[target.Id][outputIndex] = endBuilding;
        new BuildingConnector(fakeBuildingContext).ConnectBuildings(outputIndex, 0, endBuilding, target);

        return endBuilding;
    }

    private void DisconnectMinerFromTarget(FakeBuildingContextProvider fakeBuildingContext, 
        OutputBuilding disconnected, uint inputNumber)
    {
        const uint outputIndex = 0;
        fakeBuildingContext.Buildings[target.Id][outputIndex] = null;
        new BuildingConnector(fakeBuildingContext).DisconnectBuilding(outputIndex, inputNumber, target, disconnected);
    }
}