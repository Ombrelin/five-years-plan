using CommunityToolkit.Mvvm.ComponentModel;
using FiveYearPlans.ViewModels.Buildings.Abstractions;
using FiveYearPlans.ViewModels.Recipes;
using FiveYearPlans.ViewModels.Resources;

namespace FiveYearPlans.ViewModels.Buildings.ViewModels;

public partial class SmelterViewModel : OneToOneMappingBuilding
{
    public override List<Recipe> PossibleRecipes { get; } =
    [
        new Recipe(new ResourceFlow(Resource.CateriumOre, 45), new ResourceFlow(Resource.CateriumIngot, 15)),
        new Recipe(new ResourceFlow(Resource.CopperOre, 30), new ResourceFlow(Resource.CopperIngot, 30)),
        new Recipe(new ResourceFlow(Resource.IronOre, 30), new ResourceFlow(Resource.IronIngot, 30)),
    ];
}