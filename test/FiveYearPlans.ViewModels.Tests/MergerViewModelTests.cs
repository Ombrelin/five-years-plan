using System.Collections.ObjectModel;
using FiveYearPlans.ViewModels.Buildings;
using FiveYearPlans.ViewModels.Buildings.Abstractions;
using FiveYearPlans.ViewModels.Buildings.ViewModels;
using FiveYearPlans.ViewModels.Recipes;
using FiveYearPlans.ViewModels.Resources;
using FiveYearPlans.ViewModels.Tests.Fakes;
using NSubstitute;

namespace FiveYearPlans.ViewModels.Tests;

public class MergerViewModelTests
{
    private readonly MergerViewModel target = new();

    [Fact]
    public void Connect_NoOutputFlowConnected_OtherInputsAtZeroAndOutputHasFullInput()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();

        // When
        ConnectMinerToTarget(fakeBuildingContext, 0);

        // Then
        Assert.Equal(new ResourceFlow(Resource.IronOre, 30), target.InputResourceFlows[0]);
        Assert.Null(target.InputResourceFlows[1]);
        Assert.Null(target.InputResourceFlows[2]);

        Assert.Equal(new ResourceFlow(Resource.IronOre, 30), target.OutPutResourceFlows[0]);
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
        Assert.Equal(new ResourceFlow(Resource.IronOre, 30), target.InputResourceFlows[0]);
        Assert.Null(target.InputResourceFlows[1]);
        Assert.Null(target.InputResourceFlows[2]);
        
        Assert.Equal(new ResourceFlow(Resource.IronOre, 30), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 30), endBuilding.RecomputedResourceFlow);
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
        Assert.Equal(new ResourceFlow(Resource.IronOre, 60), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 60), endBuilding.RecomputedResourceFlow);
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
        Assert.Equal(new ResourceFlow(Resource.IronOre, 60), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 60), endBuilding.RecomputedResourceFlow);
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
        Assert.Equal(new ResourceFlow(Resource.IronOre, 60), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 60), endBuilding.RecomputedResourceFlow);
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
        Assert.Equal(new ResourceFlow(Resource.IronOre, 90), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 90), endBuilding.RecomputedResourceFlow);
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
        Assert.Equal(new ResourceFlow(Resource.IronOre, 90), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 90), endBuilding.RecomputedResourceFlow);
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
        Assert.Equal(new ResourceFlow(Resource.IronOre, 90), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 90), endBuilding.RecomputedResourceFlow);
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
        Assert.Equal(new ResourceFlow(Resource.IronOre, 90), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 90), endBuilding.RecomputedResourceFlow);
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
        Assert.Equal(new ResourceFlow(Resource.IronOre, 90), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 90), endBuilding.RecomputedResourceFlow);
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
        Assert.Equal(new ResourceFlow(Resource.IronOre, 90), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 90), endBuilding.RecomputedResourceFlow);
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
        Assert.Equal(new ResourceFlow(Resource.IronOre, 60), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 60), endBuilding.RecomputedResourceFlow);
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
        Assert.Equal(new ResourceFlow(Resource.IronOre, 30), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 30), endBuilding.RecomputedResourceFlow);
    }

    [Fact]
    public void Disconnect_OneInputFlowConnectedRestoreToZeroOutput_FullOnOutput()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();
        var miner = ConnectMinerToTarget(fakeBuildingContext, 1);
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);

        // When
        DisconnectMinerFromTarget(fakeBuildingContext, miner, 1);

        // Then
        Assert.Equal(new ResourceFlow(Resource.Nothing, 0), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.Nothing, 0), endBuilding.RecomputedResourceFlow);
    }


    private FakeBuildingContextProvider FakeBuildingContextProviderWithTarget() =>
        new()
        {
            OutputBuildings =
            {
                [target.Id] = new Dictionary<uint, Building?>
                {
                    [0] = null
                }
            },
            InputBuildings =
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
        fakeBuildingContextProvider.OutputBuildings[miner.Id] = new Dictionary<uint, Building?>
        {
            [0] = target
        };
        fakeBuildingContextProvider.InputBuildings[target.Id][inputNumber] = miner;
        new BuildingConnector(fakeBuildingContextProvider).ConnectBuildings(0, inputNumber, target, miner);

        return miner;
    }

    public MinerViewModel BuildIronOreMiner() =>
        new()
        {
            Resource = Resource.IronOre,
            ResourceDepositPurity = ResourceDepositPurity.Impure,
            Tier = MinerViewModel.MinerTier.Mk1
        };



    private EndBuilding ConnectEndBuildingToTarget(FakeBuildingContextProvider fakeBuildingContext)
    {
        const uint outputIndex = 0;
        var endBuilding = new EndBuilding();
        fakeBuildingContext.OutputBuildings[endBuilding.Id] = new Dictionary<uint, Building?>();
        fakeBuildingContext.OutputBuildings[target.Id][outputIndex] = endBuilding;
        
        fakeBuildingContext.InputBuildings[endBuilding.Id] = new Dictionary<uint, Building?>();
        fakeBuildingContext.InputBuildings[endBuilding.Id][0] = target;
        
        new BuildingConnector(fakeBuildingContext).ConnectBuildings(outputIndex, 0, endBuilding, target);

        return endBuilding;
    }

    private void DisconnectMinerFromTarget(FakeBuildingContextProvider fakeBuildingContext, 
        OutputBuilding disconnected, uint inputNumber)
    {
        const uint outputIndex = 0;
        fakeBuildingContext.OutputBuildings[disconnected.Id][outputIndex] = null;
        fakeBuildingContext.InputBuildings[target.Id][inputNumber] = null;
        new BuildingConnector(fakeBuildingContext).DisconnectBuilding(outputIndex, inputNumber, target, disconnected);
    }
}