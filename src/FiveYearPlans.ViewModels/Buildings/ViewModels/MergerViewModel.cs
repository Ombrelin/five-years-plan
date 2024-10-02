using CommunityToolkit.Mvvm.ComponentModel;
using FiveYearPlans.ViewModels.Buildings.Interfaces;

namespace FiveYearPlans.ViewModels.Buildings.ViewModels;

public class MergerViewModel : DynamicFlowBuilding
{
    [ObservableProperty] private ResourceFlow? outPutResourceFlow;
    [ObservableProperty] private ResourceFlow? inputResourceFlow1;
    [ObservableProperty] private ResourceFlow? inputResourceFlow2;
    [ObservableProperty] private ResourceFlow? inputResourceFlow3;

    private Dictionary<uint, ResourceFlow?> outPutResourceFlows = new()
    {
        [0] = null
    };

    
    
    public override Dictionary<uint, ResourceFlow> OutPutResourceFlows { get; }
    protected override void ComputeOutputFromInput(KeyValuePair<uint, Building>[] connectedOutputs, IReadOnlyDictionary<uint, Building?> outputConnectionState)
    {
        throw new NotImplementedException();
    }

    protected override void UpdateFlows()
    {
        throw new NotImplementedException();
    }

    protected override void EmptyOutput()
    {
        throw new NotImplementedException();
    }

    public override Dictionary<uint, ResourceFlow> InputResourceFlows { get; } = new();
    protected override bool FlowsUpdateNeeded()
    {
        throw new NotImplementedException();
    }
}