using System.Collections.ObjectModel;
using FiveYearPlans.ViewModels.Buildings;
using FiveYearPlans.ViewModels.Buildings.Abstractions;
using FiveYearPlans.ViewModels.Buildings.ViewModels;
using FiveYearPlans.ViewModels.Recipes;
using FiveYearPlans.ViewModels.Resources;
using FiveYearPlans.ViewModels.Tests.Fakes;

namespace FiveYearPlans.ViewModels.Tests;

public class BuildingTestHelper
{
    private readonly DynamicFlowBuilding target;

    public BuildingTestHelper(DynamicFlowBuilding target)
    {
        this.target = target;
    }

    public FakeBuildingContextProvider FakeBuildingContextProviderWithTarget() =>
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
            }
        };

    public void ConnectMinerToTarget(FakeBuildingContextProvider fakeBuildingContextProvider)
    {
        MinerViewModel miner = BuildIronOreMiner();
        fakeBuildingContextProvider.OutputBuildings[miner.Id] = new Dictionary<uint, Building?>
        {
            [0] = target
        };
        fakeBuildingContextProvider.InputBuildings[target.Id] = new Dictionary<uint, Building>
        {
            [0] = miner
        };
        new BuildingConnector(fakeBuildingContextProvider).ConnectBuildings(0, 0, target, miner);
    }

    public void ConnectorArbitraryProducerToTarget(FakeBuildingContextProvider fakeBuildingContextProvider, Resource resource = Resource.IronIngot, decimal quantity = 30)
    {
        var arbitraryInput = new ArbitraryProducerBuilding(new ResourceFlow(resource, quantity));
        fakeBuildingContextProvider.OutputBuildings[arbitraryInput.Id] = new Dictionary<uint, Building>
        {
            [0] = target
        };
        fakeBuildingContextProvider.InputBuildings[target.Id] = new Dictionary<uint, Building>
        {
            [0] = arbitraryInput
        };
        new BuildingConnector(fakeBuildingContextProvider).ConnectBuildings(0, 0, target, arbitraryInput);
    }

    public MinerViewModel BuildIronOreMiner() =>
        new()
        {
            Resource = Resource.IronOre,
            ResourceDepositPurity = ResourceDepositPurity.Impure,
            Tier = MinerViewModel.MinerTier.Mk1
        };

    public EndBuilding ConnectEndBuildingToTarget(FakeBuildingContextProvider fakeBuildingContext)
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

    public void DisconnectEndBuildingFromTarget(FakeBuildingContextProvider fakeBuildingContext, uint outputIndex,
        InputBuilding disconnected)
    {
        fakeBuildingContext.OutputBuildings[target.Id][outputIndex] = null;
        new BuildingConnector(fakeBuildingContext).DisconnectBuilding(outputIndex, 0, disconnected, target);
    }
}