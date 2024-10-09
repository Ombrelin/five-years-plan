using CommunityToolkit.Mvvm.ComponentModel;
using FiveYearPlans.ViewModels.Buildings.Abstractions;
using FiveYearPlans.ViewModels.Resources;

namespace FiveYearPlans.ViewModels.Buildings.ViewModels;

[ObservableObject]
public partial class SplitterViewModel : DynamicFlowBuilding
{
    [ObservableProperty] private ResourceFlow? outPutResourceFlow1;
    [ObservableProperty] private ResourceFlow? outPutResourceFlow2;
    [ObservableProperty] private ResourceFlow? outPutResourceFlow3;

    [ObservableProperty] private ResourceFlow? inputResourceFlow;

    private Dictionary<uint, ResourceFlow?> outPutResourceFlows = new()
    {
        [0] = null,
        [1] = null,
        [2] = null,
    };


    public override Dictionary<uint, ResourceFlow> OutPutResourceFlows => outPutResourceFlows;

    public override Dictionary<uint, ResourceFlow> InputResourceFlows { get; } = new();


    protected override bool FlowsUpdateNeeded() => OutPutResourceFlows[0] != OutPutResourceFlow1 ||
                                                   OutPutResourceFlows[1] != OutPutResourceFlow2 ||
                                                   OutPutResourceFlows[2] != OutPutResourceFlow3;

    protected override void UpdateFlows()
    {
        outPutResourceFlows = new Dictionary<uint, ResourceFlow?>()
        {
            [0] = OutPutResourceFlow1,
            [1] = OutPutResourceFlow2,
            [2] = OutPutResourceFlow3,
        };
    }

    protected override void ComputeOutputFromInput(KeyValuePair<uint, Building>[] connectedOutputs,
        IReadOnlyDictionary<uint, Building?> outputConnectionState)
    {
        InputResourceFlow = InputResourceFlows.Single().Value;

        if (!connectedOutputs.Any())
        {
            OutPutResourceFlow1 = new ResourceFlow(Resource.Nothing, 0);
            OutPutResourceFlow2 = new ResourceFlow(Resource.Nothing, 0);
            OutPutResourceFlow3 = new ResourceFlow(Resource.Nothing, 0);
        }
        else
        {
            var outputQuantity = (InputResourceFlow?.Quantity ?? 0) / connectedOutputs.Length;

            OutPutResourceFlow1 = BuildOutputResourceFlow(outputConnectionState[0], outputQuantity);
            OutPutResourceFlow2 = BuildOutputResourceFlow(outputConnectionState[1], outputQuantity);
            OutPutResourceFlow3 = BuildOutputResourceFlow(outputConnectionState[2], outputQuantity);
        }
    }

    private ResourceFlow BuildOutputResourceFlow(Building? building, decimal outputQuantity)
    {
        if (outputQuantity == 0 || building is null || InputResourceFlow is null)
        {
            return new ResourceFlow(Resource.Nothing, 0);
        }

        return InputResourceFlow with { Quantity = outputQuantity };
    }

    protected override void EmptyOutput()
    {
        OutPutResourceFlow1 = null;
        OutPutResourceFlow2 = null;
        OutPutResourceFlow3 = null;
    }
}