using CommunityToolkit.Mvvm.ComponentModel;
using FiveYearPlans.ViewModels.Recipes;
using FiveYearPlans.ViewModels.Resources;

namespace FiveYearPlans.ViewModels.Buildings.Abstractions;

[ObservableObject]
public abstract partial class OneToOneMappingBuilding : DynamicFlowBuilding
{
 
    public abstract List<Recipe> PossibleRecipes { get; }
 
    [ObservableProperty] private ResourceFlow? outPutResourceFlow;
    [ObservableProperty] private ResourceFlow? inputResourceFlow;
    [ObservableProperty] private Recipe? recipe;

    private Dictionary<uint, ResourceFlow?> outPutResourceFlows = new()
    {
        [0] = null
    };

    public override Dictionary<uint, ResourceFlow> OutPutResourceFlows => outPutResourceFlows;
    protected override bool FlowsUpdateNeeded() => OutPutResourceFlows[0] != OutPutResourceFlow;

    protected override void ComputeOutputFromInput(KeyValuePair<uint, Building>[] connectedOutputs,
        IReadOnlyDictionary<uint, Building?> outputConnectionState)
    {
        InputResourceFlow = InputResourceFlows.Single().Value;

        if (InputResourceFlow is null)
        {
            OutPutResourceFlow =  new ResourceFlow(Resource.Nothing, 0);
        }
        else
        {
            var inputRatio = InputResourceFlow.Quantity / Recipe?.IngredientFlow.Quantity;
            if (inputRatio > 1)
            {
                inputRatio = 1;
            }
            var outputQuantity =  Math.Round(inputRatio * Recipe?.ProductFlow.Quantity ?? 0, 2);

            OutPutResourceFlow =
                new ResourceFlow(Recipe?.ProductFlow.Resource?? Resource.Nothing, outputQuantity);
        }
    }

    protected override void UpdateFlows()
    {
        outPutResourceFlows = new Dictionary<uint, ResourceFlow?>()
        {
            [0] = OutPutResourceFlow
        };
    }

    protected override void EmptyOutput()
    {
        OutPutResourceFlow = null;
    }

    public override Dictionary<uint, ResourceFlow> InputResourceFlows { get; } = new();

}