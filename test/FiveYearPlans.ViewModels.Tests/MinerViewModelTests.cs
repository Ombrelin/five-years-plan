using System.Collections.ObjectModel;
using FiveYearPlans.ViewModels.Buildings;
using FiveYearPlans.ViewModels.Buildings.ViewModels;
using FiveYearPlans.ViewModels.Recipes;
using FiveYearPlans.ViewModels.Tests.Fakes;

namespace FiveYearPlans.ViewModels.Tests;

public class MinerViewModelTests
{
    private readonly MinerViewModel target = new()
    {
        PossibleRecipes = new ObservableCollection<Recipe>
        {
            new(
                "Limestone",
                Array.Empty<ResourceFlow>(),
                new[]
                {
                    new ResourceFlow(new Resource("Limestone"), 30)
                }
            )
        }
    };

    [Fact]
    public void Miner_Constructor_SetsDefaultRecipe()
    {
        // Then
        Assert.NotNull(target.Recipe);
        Assert.Equal(30, target.OutPutResourceFlow.Quantity);
        Assert.Equal(new Resource("Limestone"), target.OutPutResourceFlow.Resource);
        Assert.Equal(target.OutPutResourceFlow, Assert.Single(target.OutPutResourceFlows).Value);
        Assert.Equal(0u, Assert.Single(target.OutPutResourceFlows).Key);
    }

    [Fact]
    public void Miner_OutputsResourceFromRecipe()
    {
        // Arrange
        var resource = new Resource("Iron Ore");
        const int quantity = 30;
        var recipe = new Recipe(
            "Iron Ore",
            Array.Empty<ResourceFlow>(),
            new[] { new ResourceFlow(resource, quantity) }
        );

        // Act
        target.Recipe = recipe;

        // Then
        Assert.Equal(quantity, target.OutPutResourceFlow.Quantity);
        Assert.Equal(resource, target.OutPutResourceFlow.Resource);
        Assert.Equal(target.OutPutResourceFlow, Assert.Single(target.OutPutResourceFlows).Value);
        Assert.Equal(0u, Assert.Single(target.OutPutResourceFlows).Key);
    }

    [Fact]
    public void Miner_UpdateRecipe_UpdatesOutput()
    {
        // Arrange
        var resource = new Resource("Iron Ore");
        const int quantity = 30;
        var resourceFlow = new ResourceFlow(resource, quantity);
        var recipe = new Recipe(
            "Iron Ore",
            Array.Empty<ResourceFlow>(),
            new[] { resourceFlow }
        );

        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);

        // Act
        target.Recipe = recipe;

        // Then
        Assert.Equal(resourceFlow, endBuilding.RecomputedResourceFlow);
    }

    private EndBuilding ConnectEndBuildingToTarget(FakeBuildingContextProvider fakeBuildingContext,
        uint outputIndex = 0)
    {
        var endBuilding = new EndBuilding();
        fakeBuildingContext.OutputBuildings[endBuilding.Id] = new Dictionary<uint, Building?>();
        fakeBuildingContext.OutputBuildings[target.Id][outputIndex] = endBuilding;

        fakeBuildingContext.InputBuildings[endBuilding.Id] = new Dictionary<uint, Building>();
        fakeBuildingContext.InputBuildings[endBuilding.Id][0] = target;
        new BuildingConnector(fakeBuildingContext).ConnectBuildings(outputIndex, 0, endBuilding, target);

        return endBuilding;
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
            }
        };
}