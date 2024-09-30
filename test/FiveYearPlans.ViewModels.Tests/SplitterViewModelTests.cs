using System.Collections.ObjectModel;
using FiveYearPlans.ViewModels.Buildings;
using FiveYearPlans.ViewModels.Buildings.ViewModels;
using FiveYearPlans.ViewModels.Recipes;
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
    public void Connect_NoOutputFlowConnected_AllOutputsAtZero()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();

        // When
        ConnectMinerToTarget(fakeBuildingContext);

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
    public void Connect_OneOutputFlowConnected_FullOutputFlowFromInputOnIt()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();
        ConnectMinerToTarget(fakeBuildingContext);

        // When
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);
        ;

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 30), target.InputResourceFlows[0]);

        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 30), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 30), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlow3);

        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 30), endBuilding.RecomputedResourceFlow);
    }


    [Fact]
    public void Connect_TwoOutputFlowConnectedCase1_SplitInputOnEach()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();

        ConnectMinerToTarget(fakeBuildingContext);
        EndBuilding previousEndBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);

        // When
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext, 2);

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


    [Fact]
    public void Connect_TwoOutputFlowConnectedCase2_SplitInputOnEach()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();

        ConnectMinerToTarget(fakeBuildingContext);
        EndBuilding previousEndBuilding = ConnectEndBuildingToTarget(fakeBuildingContext, 1);

        // When
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext, 2);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), target.OutPutResourceFlow3);

        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), previousEndBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), endBuilding.RecomputedResourceFlow);
    }

    [Fact]
    public void Connect_TwoOutputFlowConnectedCase3_SplitInputOnEach()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();

        ConnectMinerToTarget(fakeBuildingContext);
        EndBuilding previousEndBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);

        // When
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext, 1);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlow3);

        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), previousEndBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), endBuilding.RecomputedResourceFlow);
    }


    [Fact]
    public void Connect_ThreeOutputFlowConnectedOrder1_SplitInputOnEach()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();
        ConnectMinerToTarget(fakeBuildingContext);
        EndBuilding previousEndBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext, 1);

        // When
        EndBuilding thirdEndBuilding = ConnectEndBuildingToTarget(fakeBuildingContext, 2);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlow3);

        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), previousEndBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), endBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), thirdEndBuilding.RecomputedResourceFlow);
    }

    [Fact]
    public void Connect_ThreeOutputFlowConnectedOrder2_SplitInputOnEach()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();
        ConnectMinerToTarget(fakeBuildingContext);
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext, 1);
        EndBuilding previousEndBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);

        // When
        EndBuilding thirdEndBuilding = ConnectEndBuildingToTarget(fakeBuildingContext, 2);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlow3);

        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), previousEndBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), endBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), thirdEndBuilding.RecomputedResourceFlow);
    }

    [Fact]
    public void Connect_ThreeOutputFlowConnectedOrder3_SplitInputOnEach()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();
        ConnectMinerToTarget(fakeBuildingContext);
        EndBuilding thirdEndBuilding = ConnectEndBuildingToTarget(fakeBuildingContext, 2);
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext, 1);

        // When
        EndBuilding previousEndBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlow3);

        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), previousEndBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), endBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), thirdEndBuilding.RecomputedResourceFlow);
    }

    [Fact]
    public void Connect_ThreeOutputFlowConnectedOrder4_SplitInputOnEach()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();
        ConnectMinerToTarget(fakeBuildingContext);
        EndBuilding thirdEndBuilding = ConnectEndBuildingToTarget(fakeBuildingContext, 2);
        EndBuilding previousEndBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);

        // When
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext, 1);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlow3);

        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), previousEndBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), endBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), thirdEndBuilding.RecomputedResourceFlow);
    }

    [Fact]
    public void Connect_ThreeOutputFlowConnectedOrder5_SplitInputOnEach()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();
        ConnectMinerToTarget(fakeBuildingContext);
        EndBuilding previousEndBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);
        EndBuilding thirdEndBuilding = ConnectEndBuildingToTarget(fakeBuildingContext, 2);

        // When
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext, 1);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlow3);

        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), previousEndBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), endBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), thirdEndBuilding.RecomputedResourceFlow);
    }


    [Fact]
    public void Connect_ThreeOutputFlowConnectedOrder6_SplitInputOnEach()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();
        ConnectMinerToTarget(fakeBuildingContext);
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext, 1);
        EndBuilding thirdEndBuilding = ConnectEndBuildingToTarget(fakeBuildingContext, 2);


        // When
        EndBuilding previousEndBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), target.OutPutResourceFlow3);

        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), previousEndBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), endBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 10), thirdEndBuilding.RecomputedResourceFlow);
    }

    [Fact]
    public void Disconnect_ThreeOutputFlowConnectedRestoreToTwo_SplitInputOnEach()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();
        ConnectMinerToTarget(fakeBuildingContext);
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext, 1);
        EndBuilding thirdEndBuilding = ConnectEndBuildingToTarget(fakeBuildingContext, 2);
        EndBuilding previousEndBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);

        // When
        DisconnectEndBuildingFromTarget(fakeBuildingContext, 0, previousEndBuilding);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), target.OutPutResourceFlow3);

        Assert.Equal(new ResourceFlow(new Resource("Nothing"), 0), previousEndBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), endBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 15), thirdEndBuilding.RecomputedResourceFlow);
    }

    [Fact]
    public void Disconnect_TwoOutputFlowConnectedRestoreToOne_SplitInputOnEach()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();
        ConnectMinerToTarget(fakeBuildingContext);
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext, 1);
        EndBuilding thirdEndBuilding = ConnectEndBuildingToTarget(fakeBuildingContext, 2);

        // When
        DisconnectEndBuildingFromTarget(fakeBuildingContext, 2, thirdEndBuilding);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 30), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 30), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlow3);

        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 30), endBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(new Resource("Nothing"), 0), thirdEndBuilding.RecomputedResourceFlow);
    }

    [Fact]
    public void Disconnect_OneOutputFlowConnectedRestoreToZeroOutput_SplitInputOnEach()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();
        ConnectMinerToTarget(fakeBuildingContext);
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext, 1);

        // When
        DisconnectEndBuildingFromTarget(fakeBuildingContext, 1, endBuilding);

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 0), target.OutPutResourceFlow3);

        Assert.Equal(new ResourceFlow(new Resource("Nothing"), 0), endBuilding.RecomputedResourceFlow);
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

    private void ConnectMinerToTarget(FakeBuildingContextProvider fakeBuildingContextProvider)
    {
        MinerViewModel miner = BuildIronOreMiner();
        fakeBuildingContextProvider.Buildings[miner.Id] = new Dictionary<uint, Building?>
        {
            [0] = target
        };
        new BuildingConnector(fakeBuildingContextProvider).ConnectBuildings(0, 0, target, miner);
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


    private EndBuilding ConnectEndBuildingToTarget(FakeBuildingContextProvider fakeBuildingContext,
        uint outputIndex = 0)
    {
        var endBuilding = new EndBuilding();
        fakeBuildingContext.Buildings[endBuilding.Id] = new Dictionary<uint, Building?>();
        fakeBuildingContext.Buildings[target.Id][outputIndex] = endBuilding;
        new BuildingConnector(fakeBuildingContext).ConnectBuildings(outputIndex, 0, endBuilding, target);

        return endBuilding;
    }

    private void DisconnectEndBuildingFromTarget(FakeBuildingContextProvider fakeBuildingContext, uint outputIndex,
        InputBuilding disconnected)
    {
        fakeBuildingContext.Buildings[target.Id][outputIndex] = null;
        new BuildingConnector(fakeBuildingContext).DisconnectBuilding(outputIndex, 0, disconnected, target);
    }
}