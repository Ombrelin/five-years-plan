using CommunityToolkit.Mvvm.ComponentModel;
using FiveYearPlans.ViewModels.Buildings.Abstractions;
using FiveYearPlans.ViewModels.Recipes;
using FiveYearPlans.ViewModels.Resources;

namespace FiveYearPlans.ViewModels.Buildings.ViewModels;

public partial class ConstructorViewModel : OneToOneMappingBuilding
{
    public override List<Recipe> PossibleRecipes { get; } =
    [
        new Recipe(new ResourceFlow(Resource.Leaves, 120), new ResourceFlow(Resource.Biomass, 60)),
        new Recipe(new ResourceFlow(Resource.IronIngot, 30), new ResourceFlow(Resource.IronPlate, 20)),
        new Recipe(new ResourceFlow(Resource.IronIngot, 15), new ResourceFlow(Resource.IronRod, 15)),
        new Recipe(new ResourceFlow(Resource.IronRod, 10), new ResourceFlow(Resource.Screw, 40)),
        new Recipe(new ResourceFlow(Resource.CopperIngot, 15), new ResourceFlow(Resource.Wire, 30))
    ];
}