using CommunityToolkit.Mvvm.ComponentModel;
using FiveYearPlans.ViewModels.Buildings.Interfaces;

namespace FiveYearPlans.ViewModels.Buildings.ViewModels;

[ObservableObject]
public partial class MergerViewModel : DynamicFlowBuilding
{
    [ObservableProperty] private ResourceFlow? outPutResourceFlow;
    [ObservableProperty] private ResourceFlow? inputResourceFlow1;
    [ObservableProperty] private ResourceFlow? inputResourceFlow2;
    [ObservableProperty] private ResourceFlow? inputResourceFlow3;

    private Dictionary<uint, ResourceFlow?> outPutResourceFlows = new()
    {
        [0] = null
    };

    private readonly Dictionary<uint, ResourceFlow?> inPutResourceFlows = new()
    {
        [0] = null,
        [1] = null,
        [2] = null
    };


    public override Dictionary<uint, ResourceFlow> OutPutResourceFlows => outPutResourceFlows;
    public override Dictionary<uint, ResourceFlow> InputResourceFlows => inPutResourceFlows;

    protected override void ComputeOutputFromInput(KeyValuePair<uint, Building>[] connectedOutputs,
        IReadOnlyDictionary<uint, Building?> outputConnectionState)
    {
        InputResourceFlow1 = InputResourceFlows[0];
        InputResourceFlow2 = InputResourceFlows[1];
        InputResourceFlow3 = InputResourceFlows[2];
        
        var resourceFlows = new[] { InputResourceFlow1, InputResourceFlow2, InputResourceFlow3 };
        var resource = resourceFlows.FirstOrDefault(resourceFlow => resourceFlow is not null && resourceFlow.Resource.Name != "Nothing")?.Resource;
        if (resource is null)
        {
            OutPutResourceFlow = new ResourceFlow(new Resource("Nothing"), 0);
            return;
        }
        
        if (resourceFlows.Any(resourceFlow =>
                resourceFlow is not null && resourceFlow.Resource.Name != "Nothing" && resourceFlow.Resource != resource))
        {
            throw new InvalidOperationException("Different resources are not supported yet.");
        }

        var outputCount = (InputResourceFlow1?.Quantity ?? 0) + (InputResourceFlow2?.Quantity ?? 0) +
            (InputResourceFlow3?.Quantity ?? 0);

        if (!connectedOutputs.Any())
        {
            OutPutResourceFlow = new ResourceFlow(resource, 0);
        }
        else
        {
            OutPutResourceFlow = new ResourceFlow(resource, outputCount);
        }
    }

    protected override void UpdateFlows()
    {
        outPutResourceFlows = new Dictionary<uint, ResourceFlow?>()
        {
            [0] = OutPutResourceFlow,
        };
    }

    protected override void EmptyOutput()
    {
        OutPutResourceFlow = null;
    }


    protected override bool FlowsUpdateNeeded() => OutPutResourceFlows[0] != OutPutResourceFlow;
}