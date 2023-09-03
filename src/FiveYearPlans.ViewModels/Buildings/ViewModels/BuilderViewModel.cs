using CommunityToolkit.Mvvm.ComponentModel;
using FiveYearPlans.ViewModels.Buildings.Interfaces;

namespace FiveYearPlans.ViewModels.Buildings.ViewModels;

[ObservableObject]
public partial class BuilderViewModel : DynamicFlowBuilding, OutputBuilding
{
    [ObservableProperty] private ResourceFlow? outPutResourceFlow;
    [ObservableProperty] private ResourceFlow? inputResourceFlow;

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

        if (!connectedOutputs.Any())
        {
            OutPutResourceFlow = InputResourceFlow with { Quantity = 0 };
        }
        else
        {
            var outputQuantity = InputResourceFlow?.Quantity ?? 0;

            OutPutResourceFlow =
                new ResourceFlow(InputResourceFlow?.Resource ?? new Resource("Nothing"),
                    outputConnectionState[0] is not null ? outputQuantity : 0);
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