using FiveYearPlans.ViewModels.Buildings;
using FiveYearPlans.ViewModels.Buildings.Abstractions;
using FiveYearPlans.ViewModels.Resources;

namespace FiveYearPlans.ViewModels.Tests.Fakes;

public class EndBuilding : DynamicFlowBuilding, InputBuilding
{
    protected override void ComputeOutputFromInput(KeyValuePair<uint, Building>[] connectedOutputs,
        IReadOnlyDictionary<uint, Building?> outputConnectionState)
    {
        KeyValuePair<uint, ResourceFlow>? input = InputResourceFlows.SingleOrDefault();
        if (input is not null)
        {
            RecomputedResourceFlow = input.Value.Value;
        }
    }

    protected override void UpdateFlows()
    {
        //throw new NotImplementedException();
    }

    protected override void EmptyOutput()
    {
        RecomputedResourceFlow = null;
    }

    public override Dictionary<uint, ResourceFlow> InputResourceFlows { get; } = new();

    public override Dictionary<uint, ResourceFlow> OutPutResourceFlows { get; } =
        new Dictionary<uint, ResourceFlow?>();

    public ResourceFlow? RecomputedResourceFlow { get; private set; }

    protected override bool FlowsUpdateNeeded() => true;
}