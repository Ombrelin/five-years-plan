namespace FiveYearPlans.ViewModels.Buildings.Interfaces;

public abstract class DynamicFlowBuilding : Building, InputBuilding, OutputBuilding
{
    protected IBuildingContextProvider? EmbeddedBuildingContextProvider;

    protected abstract void ComputeOutputFromInput(KeyValuePair<uint, Building>[] connectedOutputs,
        IReadOnlyDictionary<uint, Building?> outputConnectionState);

    protected abstract void UpdateFlows();
    protected abstract void EmptyOutput();

    public abstract Dictionary<uint, ResourceFlow> InputResourceFlows { get; }


    protected abstract bool FlowsUpdateNeeded();

    public void RecomputeOutput(IBuildingContextProvider buildingContextProvider)
    {
        this.EmbeddedBuildingContextProvider = buildingContextProvider;
        IReadOnlyDictionary<uint, Building?> outputConnectionState =
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


    private static KeyValuePair<uint, Building>[] GetConnectedOutputs(
        IReadOnlyDictionary<uint, Building?> outputConnectionState)
    {
        var connectedOutputs = outputConnectionState
            .Where(kvp => kvp.Value is not null)
            .ToArray() as KeyValuePair<uint, Building>[];
        return connectedOutputs;
    }
}