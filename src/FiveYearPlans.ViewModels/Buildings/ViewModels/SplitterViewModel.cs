using CommunityToolkit.Mvvm.ComponentModel;
using FiveYearPlans.ViewModels.Buildings.Interfaces;

namespace FiveYearPlans.ViewModels.Buildings.ViewModels;

[ObservableObject]
public partial class SplitterViewModel : DynamicFlowBuilding, OutputBuilding
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
            OutPutResourceFlow1 = InputResourceFlow with { Quantity = 0 };
            OutPutResourceFlow2 = InputResourceFlow with { Quantity = 0 };
            OutPutResourceFlow3 = InputResourceFlow with { Quantity = 0 };
        }
        else
        {
            var outputQuantity = (InputResourceFlow?.Quantity ?? 0) / connectedOutputs.Length;

            OutPutResourceFlow1 =
                new ResourceFlow(InputResourceFlow?.Resource ?? new Resource("Nothing"),
                    outputConnectionState[0] is not null ? outputQuantity : 0);
            OutPutResourceFlow2 =
                new ResourceFlow(InputResourceFlow?.Resource ?? new Resource("Nothing"),
                    outputConnectionState[1] is not null ? outputQuantity : 0);
            OutPutResourceFlow3 =
                new ResourceFlow(InputResourceFlow?.Resource ?? new Resource("Nothing"),
                    outputConnectionState[2] is not null ? outputQuantity : 0);
        }
    }

    protected override void EmptyOutput()
    {
        OutPutResourceFlow1 = null;
        OutPutResourceFlow2 = null;
        OutPutResourceFlow3 = null;
    }
}