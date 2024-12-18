using System.Collections.ObjectModel;
using FiveYearPlans.ViewModels.Buildings;
using FiveYearPlans.ViewModels.Buildings.Abstractions;
using FiveYearPlans.ViewModels.Buildings.ViewModels;
using FiveYearPlans.ViewModels.Recipes;
using FiveYearPlans.ViewModels.Resources;
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
        target.InputResourceFlows[0] = new ResourceFlow(Resource.IronOre, 30);
        target.InputResourceFlows[1] = new ResourceFlow(Resource.IronOre, 30);

        // When
        var act = () => target.RecomputeOutput(Substitute.For<IBuildingContextProvider>());

        // Then
        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void Connect_NoOutputFlowConnected_SplitInputBetweenOutputs()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();

        // When
        ConnectMinerToTarget(fakeBuildingContext);

        // Then
        Assert.Equal(new ResourceFlow(Resource.IronOre, 30), target.InputResourceFlows[0]);

        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlow3);
    }

    [Fact]
    public void Connect_OneOutputFlowConnected_FullOutputFlowFromInputOnIt()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();
        ConnectMinerToTarget(fakeBuildingContext);

        // When
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);

        // Then
        Assert.Equal(new ResourceFlow(Resource.IronOre, 30), target.InputResourceFlows[0]);

        Assert.Equal(new ResourceFlow(Resource.IronOre, 30), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 30), target.OutPutResourceFlow1);
        Assert.Null(target.OutPutResourceFlows[1]);
        Assert.Null(target.OutPutResourceFlow2);
        Assert.Null(target.OutPutResourceFlows[2]);
        Assert.Null(target.OutPutResourceFlow3);

        Assert.Equal(new ResourceFlow(Resource.IronOre, 30), endBuilding.RecomputedResourceFlow);
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
        Assert.Equal(new ResourceFlow(Resource.IronOre, 15), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 15), target.OutPutResourceFlow1);
        Assert.Null(target.OutPutResourceFlows[1]);
        Assert.Null(target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 15), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 15), target.OutPutResourceFlow3);

        Assert.Equal(new ResourceFlow(Resource.IronOre, 15), previousEndBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 15), endBuilding.RecomputedResourceFlow);
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
        Assert.Null(target.OutPutResourceFlows[0]);
        Assert.Null(target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 15), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 15), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 15), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 15), target.OutPutResourceFlow3);

        Assert.Equal(new ResourceFlow(Resource.IronOre, 15), previousEndBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 15), endBuilding.RecomputedResourceFlow);
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
        Assert.Equal(new ResourceFlow(Resource.IronOre, 15), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 15), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 15), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 15), target.OutPutResourceFlow2);
        Assert.Null(target.OutPutResourceFlows[2]);
        Assert.Null(target.OutPutResourceFlow3);

        Assert.Equal(new ResourceFlow(Resource.IronOre, 15), previousEndBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 15), endBuilding.RecomputedResourceFlow);
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
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlow3);

        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), previousEndBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), endBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), thirdEndBuilding.RecomputedResourceFlow);
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
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlow3);

        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), previousEndBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), endBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), thirdEndBuilding.RecomputedResourceFlow);
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
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlow3);

        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), previousEndBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), endBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), thirdEndBuilding.RecomputedResourceFlow);
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
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlow3);

        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), previousEndBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), endBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), thirdEndBuilding.RecomputedResourceFlow);
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
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlow3);

        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), previousEndBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), endBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), thirdEndBuilding.RecomputedResourceFlow);
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
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlow3);

        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), previousEndBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), endBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), thirdEndBuilding.RecomputedResourceFlow);
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
        Assert.Null(target.OutPutResourceFlows[0]);
        Assert.Null(target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 15), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 15), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 15), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 15), target.OutPutResourceFlow3);

        Assert.Null(previousEndBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 15), endBuilding.RecomputedResourceFlow);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 15), thirdEndBuilding.RecomputedResourceFlow);
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
        Assert.Null(target.OutPutResourceFlows[0]);
        Assert.Null(target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 30), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 30), target.OutPutResourceFlow2);
        Assert.Null(target.OutPutResourceFlows[2]);
        Assert.Null(target.OutPutResourceFlow3);

        Assert.Equal(new ResourceFlow(Resource.IronOre, 30), endBuilding.RecomputedResourceFlow);
        Assert.Null(thirdEndBuilding.RecomputedResourceFlow);
    }

    [Fact]
    public void Disconnect_OneOutputFlowConnectedRestoreToZeroOutput_SplitBetweenOutputs()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();
        ConnectMinerToTarget(fakeBuildingContext);
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext, 1);

        // When
        DisconnectEndBuildingFromTarget(fakeBuildingContext, 1, endBuilding);

        // Then
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlow1);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlows[1]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlow2);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlows[2]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 10), target.OutPutResourceFlow3);

        Assert.Null(endBuilding.RecomputedResourceFlow);
    }


    private FakeBuildingContextProvider FakeBuildingContextProviderWithTarget() =>
        new()
        {
            OutputBuildings =
            {
                [target.Id] = new Dictionary<uint, Building?>
                {
                    [0] = null,
                    [1] = null,
                    [2] = null
                }
            },
            InputBuildings =
            {
                [target.Id] = new Dictionary<uint, Building?>
                {
                    [0] = null
                }
            }
        };

    private void ConnectMinerToTarget(FakeBuildingContextProvider fakeBuildingContextProvider)
    {
        MinerViewModel miner = BuildIronOreMiner();
        fakeBuildingContextProvider.OutputBuildings[miner.Id] = new Dictionary<uint, Building?>
        {
            [0] = target
        };
        fakeBuildingContextProvider.InputBuildings[target.Id][0] = miner;
        new BuildingConnector(fakeBuildingContextProvider).ConnectBuildings(0, 0, target, miner);
    }

    public MinerViewModel BuildIronOreMiner() =>
        new()
        {
            Resource = Resource.IronOre,
            ResourceDepositPurity = ResourceDepositPurity.Impure,
            Tier = MinerViewModel.MinerTier.Mk1
        };



    private EndBuilding ConnectEndBuildingToTarget(FakeBuildingContextProvider fakeBuildingContext,
        uint outputIndex = 0)
    {
        var endBuilding = new EndBuilding();
        fakeBuildingContext.OutputBuildings[endBuilding.Id] = new Dictionary<uint, Building?>();
        fakeBuildingContext.OutputBuildings[target.Id][outputIndex] = endBuilding;

        fakeBuildingContext.InputBuildings[endBuilding.Id] = new Dictionary<uint, Building?>();
        fakeBuildingContext.InputBuildings[endBuilding.Id][0] = target;
        
        new BuildingConnector(fakeBuildingContext).ConnectBuildings(outputIndex, 0, endBuilding, target);

        return endBuilding;
    }

    private void DisconnectEndBuildingFromTarget(FakeBuildingContextProvider fakeBuildingContext, uint outputIndex,
        InputBuilding disconnected)
    {
        fakeBuildingContext.OutputBuildings[target.Id][outputIndex] = null;
        fakeBuildingContext.InputBuildings[disconnected.Id][0] = null;
        new BuildingConnector(fakeBuildingContext).DisconnectBuilding(outputIndex, 0, disconnected, target);
    }
}