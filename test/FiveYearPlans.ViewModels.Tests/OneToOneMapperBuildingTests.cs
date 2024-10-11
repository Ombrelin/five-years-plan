using FiveYearPlans.ViewModels.Buildings.ViewModels;
using FiveYearPlans.ViewModels.Recipes;
using FiveYearPlans.ViewModels.Resources;
using FiveYearPlans.ViewModels.Tests.Fakes;

namespace FiveYearPlans.ViewModels.Tests;

public class OneToOneMapperBuildingTests
{
    [Fact]
    public void ConnectConstructor_NoRecipe_OutputsAtZero()
    {
        // Given
        var target = new ConstructorViewModel();
        var helper = new BuildingTestHelper(target);
        var fakeBuildingContext = helper.FakeBuildingContextProviderWithTarget();

        // When
        helper.ConnectorArbitraryProducerToTarget(fakeBuildingContext, resource: Resource.IronIngot, quantity: 30);

        // Then
        Assert.Equal(new ResourceFlow(Resource.IronIngot, 30), target.InputResourceFlows[0]);

        Assert.Single(target.OutPutResourceFlows);
        Assert.Null(target.OutPutResourceFlows[0]);
        Assert.Null(target.OutPutResourceFlow);
    }
    
    [Fact]
    public void ConnectConstructor_NoOutputFlowConnected_OutputsResultOfRecipe()
    {
        // Given
        var target = new ConstructorViewModel();
        target.Recipe = target.PossibleRecipes.Single(recipe => recipe.ProductFlow.Resource == Resource.IronPlate);
        var helper = new BuildingTestHelper(target);
        var fakeBuildingContext = helper.FakeBuildingContextProviderWithTarget();

        // When
        helper.ConnectorArbitraryProducerToTarget(fakeBuildingContext, resource: Resource.IronIngot, quantity: 30);

        // Then
        Assert.Equal(new ResourceFlow(Resource.IronIngot, 30), target.InputResourceFlows[0]);

        Assert.Single(target.OutPutResourceFlows);
        Assert.Equal(new ResourceFlow(Resource.IronPlate, 20), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronPlate, 20), target.OutPutResourceFlow);
    }

    [Theory]
    [InlineData(Resource.Leaves, 120, 120, Resource.Biomass, 60, 60)]
    [InlineData(Resource.Leaves, 120, 60, Resource.Biomass, 60, 30)]
    [InlineData(Resource.IronIngot, 30, 30, Resource.IronPlate, 20, 20)]
    [InlineData(Resource.IronIngot, 30, 10, Resource.IronPlate, 20, 6.67)]
    [InlineData(Resource.IronIngot, 15, 15, Resource.IronRod, 15, 15)]
    [InlineData(Resource.IronIngot, 15, 30, Resource.IronRod, 15, 15)]
    [InlineData(Resource.IronRod, 10, 2.5, Resource.Screw, 40, 10)]
    [InlineData(Resource.CopperIngot, 15, 15, Resource.Wire, 30, 30)]
    [InlineData(Resource.CopperIngot, 15, 3, Resource.Wire, 30, 6)]
    public void ConnectConstructor_SetRecipeAndConnectInput_ProducesAccordingToRecipe(
        Resource recipeIngredient,
        decimal recipeIngredientQuantity,
        decimal actualIngredientQuantity,
        Resource recipeProduct,
        decimal recipeProductQuantity,
        decimal expectedOutputQuantity
    )
    {
        // Given
        var target = new ConstructorViewModel();
        var helper = new BuildingTestHelper(target);
        var fakeBuildingContext = helper.FakeBuildingContextProviderWithTarget();
        target.Recipe = new Recipe(new ResourceFlow(recipeIngredient, recipeIngredientQuantity),
            new ResourceFlow(recipeProduct, recipeProductQuantity));


        // When
        helper.ConnectorArbitraryProducerToTarget(fakeBuildingContext, recipeIngredient, actualIngredientQuantity);
        helper.ConnectEndBuildingToTarget(fakeBuildingContext);

        // Then
        Assert.Equal(new ResourceFlow(recipeIngredient, actualIngredientQuantity), target.InputResourceFlows[0]);

        Assert.Equal(new ResourceFlow(recipeProduct, expectedOutputQuantity), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(recipeProduct, expectedOutputQuantity), target.OutPutResourceFlow);
    }
    
    [Fact]
    public void ConnectSmelter_NoRecipe_OutputsAtZero()
    {
        // Given
        var target = new SmelterViewModel();
        var helper = new BuildingTestHelper(target);
        var fakeBuildingContext = helper.FakeBuildingContextProviderWithTarget();

        // When
        helper.ConnectMinerToTarget(fakeBuildingContext);
        

        // Then
        Assert.Equal(new ResourceFlow(Resource.IronOre, 30), target.InputResourceFlows[0]);

        Assert.Null(target.OutPutResourceFlows[0]);
        Assert.Null(target.OutPutResourceFlow);
    }
    
    [Fact]
    public void ConnectSmelter_NoOutputFlowConnected_OutputsResultOfRecipe()
    {
        // Given
        var target = new SmelterViewModel();
        target.Recipe = target.PossibleRecipes.Single(recipe => recipe.ProductFlow.Resource == Resource.IronIngot);
        var helper = new BuildingTestHelper(target);
        var fakeBuildingContext = helper.FakeBuildingContextProviderWithTarget();

        // When
        helper.ConnectMinerToTarget(fakeBuildingContext);

        // Then
        Assert.Equal(new ResourceFlow(Resource.IronOre, 30), target.InputResourceFlows[0]);

        Assert.Equal(new ResourceFlow(Resource.IronIngot, 30), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronIngot, 30), target.OutPutResourceFlow);
    }

    [Theory]
    [InlineData(Resource.CateriumOre, 45, 45, Resource.CateriumIngot, 15, 15)]
    [InlineData(Resource.CateriumOre, 45, 15, Resource.CateriumIngot, 15, 5)]
    [InlineData(Resource.CopperOre, 30, 45, Resource.CopperIngot, 30, 30)]
    [InlineData(Resource.IronOre, 30, 30, Resource.IronIngot, 30, 30)]
    [InlineData(Resource.IronOre, 30, 15, Resource.IronIngot, 30, 15)]
    public void ConnectSmelter_SetRecipeAndConnectInput_ProducesAccordingToRecipe(
        Resource recipeIngredient,
        decimal recipeIngredientQuantity,
        decimal actualIngredientQuantity,
        Resource recipeProduct,
        decimal recipeProductQuantity,
        decimal expectedOutputQuantity
    )
    {
        // Given
        var target = new SmelterViewModel();
        var helper = new BuildingTestHelper(target);
        var fakeBuildingContext = helper.FakeBuildingContextProviderWithTarget();
        target.Recipe = new Recipe(new ResourceFlow(recipeIngredient, recipeIngredientQuantity),
            new ResourceFlow(recipeProduct, recipeProductQuantity));


        // When
        helper.ConnectorArbitraryProducerToTarget(fakeBuildingContext, recipeIngredient, actualIngredientQuantity);
        helper.ConnectEndBuildingToTarget(fakeBuildingContext);

        // Then
        Assert.Equal(new ResourceFlow(recipeIngredient, actualIngredientQuantity), target.InputResourceFlows[0]);

        Assert.Equal(new ResourceFlow(recipeProduct, expectedOutputQuantity), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(recipeProduct, expectedOutputQuantity), target.OutPutResourceFlow);
    }
}