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

        var outputNumbers = connectedOutputs.Length == 0 ? 3 : connectedOutputs.Length;
        var outputQuantity = (InputResourceFlow?.Quantity ?? 0) / outputNumbers;

        if (connectedOutputs.Length == 0)
        {
            OutPutResourceFlow1 = BuildOutputResourceFlow(outputQuantity, connectedOutputs);
            OutPutResourceFlow2 = BuildOutputResourceFlow(outputQuantity, connectedOutputs);
            OutPutResourceFlow3 = BuildOutputResourceFlow(outputQuantity, connectedOutputs);
        }
        else
        {
            var connectionStates = new Dictionary<uint, Building?>(outputConnectionState.Where(kvp => kvp.Value != null));

            OutPutResourceFlow1 = connectionStates.ContainsKey(0) ? BuildOutputResourceFlow(outputQuantity, connectedOutputs) : null;
            OutPutResourceFlow2 = connectionStates.ContainsKey(1) ? BuildOutputResourceFlow(outputQuantity, connectedOutputs) : null;
            OutPutResourceFlow3 = connectionStates.ContainsKey(2) ? BuildOutputResourceFlow(outputQuantity, connectedOutputs) : null;
        }
    }

    private ResourceFlow? BuildOutputResourceFlow(decimal outputQuantity, KeyValuePair<uint, Building>[]  connectedOutputs)
    {
        if (InputResourceFlow is null)
        {
            return null;
        }
        if (outputQuantity == 0 && connectedOutputs.Length != 0)
        {
            return null;
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