using CommunityToolkit.Mvvm.ComponentModel;
using FiveYearPlans.ViewModels.Buildings.Abstractions;
using FiveYearPlans.ViewModels.Resources;

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
        
        var resource = ExtractInputResource();

        var outputCount = InputResourceFlows
            .Values
            .Where(resourceFlow => resourceFlow is not null)
            .Sum(resourceFlow => resourceFlow.Quantity);

        if (!connectedOutputs.Any() || outputCount == 0)
        {
            OutPutResourceFlow = new ResourceFlow(Resource.Nothing, 0);
        }
        else
        {
            OutPutResourceFlow = new ResourceFlow(resource, outputCount);
        }
    }

    private Resource ExtractInputResource()
    {
        var resources = InputResourceFlows.Values
            .Where(resourceFlow => resourceFlow is not null)
            .Select(resourceFlow => resourceFlow.Resource)
            .Distinct()
            .ToList();

        return resources switch
        {
            [var resource, Resource.Nothing] => resource,
            [Resource.Nothing, var resource] => resource,
            [var resource] => resource,
            _ => throw new InvalidOperationException("Different resources not supported yet")
        };
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