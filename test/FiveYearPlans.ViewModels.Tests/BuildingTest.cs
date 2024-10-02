using System.Collections.ObjectModel;
using FiveYearPlans.ViewModels.Buildings;
using FiveYearPlans.ViewModels.Buildings.Interfaces;
using FiveYearPlans.ViewModels.Buildings.ViewModels;
using FiveYearPlans.ViewModels.Recipes;
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

    public EndBuilding ConnectEndBuildingToTarget(FakeBuildingContextProvider fakeBuildingContext,
        uint outputIndex = 0)
    {
        var endBuilding = new EndBuilding();
        fakeBuildingContext.OutputBuildings[target.Id][outputIndex] = endBuilding;
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