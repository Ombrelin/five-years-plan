using CommunityToolkit.Mvvm.ComponentModel;

namespace FiveYearPlans.ViewModels.Buildings.ViewModels;

[ObservableObject]
public partial class SplitterViewModel : Building, OutputBuilding, DynamicFlowBuilding
{
    [ObservableProperty] private ResourceFlow? outPutResourceFlow1;
    [ObservableProperty] private ResourceFlow? outPutResourceFlow2;
    [ObservableProperty] private ResourceFlow? outPutResourceFlow3;

    [ObservableProperty] private ResourceFlow? inputResourceFlow;


    public IReadOnlyDictionary<uint, ResourceFlow?> OutPutResourceFlows { get; private set; } =
        new Dictionary<uint, ResourceFlow?>()
        {
            [0] = null,
            [1] = null,
            [2] = null,
        };

    public Dictionary<uint, ResourceFlow> InputResourceFlows { get; } = new();

    public void RecomputeOutput(IBuildingContextProvider buildingContextProvider)
    {
        IReadOnlyDictionary<uint, DynamicFlowBuilding?> outputConnectionState =
            buildingContextProvider.GetOutputConnectionState(Id);

        var connectedOutputs = GetConnectedOutputs(outputConnectionState);

        if (!InputResourceFlows.Any())
        {
            EmptyOutput();
        }
        else
        {
            ComputeOutputFromInput(connectedOutputs, outputConnectionState);
        }

        if (FlowsUpdateNeeded())
        {
            UpdateFlows();
            RecomputeChildren(buildingContextProvider, connectedOutputs);
        }
    }

    private void RecomputeChildren(IBuildingContextProvider buildingContextProvider, KeyValuePair<uint, DynamicFlowBuilding>[] connectedOutputs)
    {
        foreach ((uint Index, DynamicFlowBuilding connectedOutput) in connectedOutputs)
        {
            connectedOutput.InputResourceFlows[0] = OutPutResourceFlows[Index]; // TODO: Handle multi input buildings
            connectedOutput.RecomputeOutput(buildingContextProvider);
        }
    }

    private bool FlowsUpdateNeeded() => OutPutResourceFlows[0] != OutPutResourceFlow1 || OutPutResourceFlows[1] != OutPutResourceFlow2|| OutPutResourceFlows[2] != OutPutResourceFlow3;

    private void UpdateFlows()
    {
        OutPutResourceFlows = new Dictionary<uint, ResourceFlow?>()
        {
            [0] = OutPutResourceFlow1,
            [1] = OutPutResourceFlow2,
            [2] = OutPutResourceFlow3,
        };
    }

    private void ComputeOutputFromInput(KeyValuePair<uint, DynamicFlowBuilding>[] connectedOutputs, IReadOnlyDictionary<uint, DynamicFlowBuilding?> outputConnectionState)
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

    private void EmptyOutput()
    {
        OutPutResourceFlow1 = null;
        OutPutResourceFlow2 = null;
        OutPutResourceFlow3 = null;
    }

    private static KeyValuePair<uint, DynamicFlowBuilding>[] GetConnectedOutputs(IReadOnlyDictionary<uint, DynamicFlowBuilding?> outputConnectionState)
    {
        var connectedOutputs = outputConnectionState
            .Where(kvp => kvp.Value is not null)
            .ToArray() as KeyValuePair<uint, DynamicFlowBuilding>[];
        return connectedOutputs;
    }
}