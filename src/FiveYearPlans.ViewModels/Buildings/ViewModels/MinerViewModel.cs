using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using FiveYearPlans.ViewModels.Buildings.Interfaces;
using FiveYearPlans.ViewModels.Recipes;
using FiveYearPlans.ViewModels.Resources;

namespace FiveYearPlans.ViewModels.Buildings.ViewModels;

[ObservableObject]
public partial class MinerViewModel : Building, OutputBuilding
{
    [ObservableProperty] private ResourceFlow outPutResourceFlow;
    [ObservableProperty] private Recipe recipe;
    [ObservableProperty] private ObservableCollection<Recipe> possibleRecipes;

    [JsonConstructor]
    public MinerViewModel()
    {
        PropertyChanged += OnPropertyChangedEventHandler;
    }

    private void OnPropertyChangedEventHandler(object? _, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == nameof(Recipe))
        {
            ReactToRecipeUpdate();
        }
        else if (args.PropertyName == nameof(PossibleRecipes))
        {
            ReactToPossibleRecipeUpdate();
        }
    }

    private void ReactToPossibleRecipeUpdate()
    {
        Recipe = PossibleRecipes.First();
    }

    private void ReactToRecipeUpdate()
    {
        this.OutPutResourceFlow = Recipe.Products.Single();
        if (buildingsProvider is not null)
        {
            RecomputeChildren(buildingsProvider, buildingsProvider.GetOutputConnectionState(Id).ToArray());
        }
    }

    public override Dictionary<uint, ResourceFlow> OutPutResourceFlows => new()
    {
        [0] = outPutResourceFlow
    };
}