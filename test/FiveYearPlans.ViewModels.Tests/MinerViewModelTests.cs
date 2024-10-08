using System.Collections.ObjectModel;
using FiveYearPlans.ViewModels.Buildings;
using FiveYearPlans.ViewModels.Buildings.ViewModels;
using FiveYearPlans.ViewModels.Recipes;
using FiveYearPlans.ViewModels.Resources;
using FiveYearPlans.ViewModels.Tests.Fakes;

namespace FiveYearPlans.ViewModels.Tests;

public class MinerViewModelTests
{
    private readonly MinerViewModel target = new();

    [Fact]
    public void Miner_Constructor_SetsDefaultRecipe()
    {
        // When
        target.Resource = Resource.Limestone;
        
        // Then
        Assert.Equal(30, target.OutPutResourceFlow.Quantity);
        Assert.Equal(ResourceDepositPurity.Impure, target.ResourceDepositPurity);
        Assert.Equal(MinerViewModel.MinerTier.Mk1, target.Tier);
        Assert.Equal(Resource.Limestone, target.OutPutResourceFlow.Resource);
        Assert.Equal(target.OutPutResourceFlow, Assert.Single(target.OutPutResourceFlows).Value);
        Assert.Equal(0u, Assert.Single(target.OutPutResourceFlows).Key);
    }
    
    [Theory]
    [InlineData(ResourceDepositPurity.Impure, MinerViewModel.MinerTier.Mk1, 30)]
    [InlineData(ResourceDepositPurity.Normal, MinerViewModel.MinerTier.Mk1, 60)]
    [InlineData(ResourceDepositPurity.Pure, MinerViewModel.MinerTier.Mk1, 120)]
    [InlineData(ResourceDepositPurity.Impure, MinerViewModel.MinerTier.Mk2, 60)]
    [InlineData(ResourceDepositPurity.Normal, MinerViewModel.MinerTier.Mk2, 120)]
    [InlineData(ResourceDepositPurity.Pure, MinerViewModel.MinerTier.Mk2, 240)]
    [InlineData(ResourceDepositPurity.Impure, MinerViewModel.MinerTier.Mk3, 120)]
    [InlineData(ResourceDepositPurity.Normal, MinerViewModel.MinerTier.Mk3, 240)]
    [InlineData(ResourceDepositPurity.Pure, MinerViewModel.MinerTier.Mk3, 480)]
    public void Miner_OutCalculation_IsCorrect(ResourceDepositPurity resourceDepositPurity, MinerViewModel.MinerTier minerTier, decimal expectedOutput)
    {
        // When
        target.Resource = Resource.Limestone;
        target.ResourceDepositPurity = resourceDepositPurity;
        target.Tier = minerTier;
        
        // Then
        Assert.Equal(expectedOutput, target.OutPutResourceFlow.Quantity);
    }
    
    [Fact]
    public void Miner_OutputsResourceFromRecipe()
    {
        // Act
        target.Resource = Resource.IronOre;

        // Then
        Assert.Equal(30, target.OutPutResourceFlow.Quantity);
        Assert.Equal(Resource.IronOre, target.OutPutResourceFlow.Resource);
        Assert.Equal(target.OutPutResourceFlow, Assert.Single(target.OutPutResourceFlows).Value);
        Assert.Equal(0u, Assert.Single(target.OutPutResourceFlows).Key);
    }

    [Fact]
    public void Miner_UpdateRecipe_UpdatesOutput()
    {
        // Arrange
        FakeBuildingContextProvider fakeBuildingContext = FakeBuildingContextProviderWithTarget();
        EndBuilding endBuilding = ConnectEndBuildingToTarget(fakeBuildingContext);

        // Act
        target.Resource = Resource.IronOre;

        // Then
        Assert.Equal(Resource.IronOre, endBuilding.RecomputedResourceFlow.Resource);
        Assert.Equal(30, endBuilding.RecomputedResourceFlow.Quantity);
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