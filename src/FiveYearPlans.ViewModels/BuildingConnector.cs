namespace FiveYearPlans.ViewModels;

public class BuildingConnector
{
    private readonly IBuildingContextProvider buildingContextProvider;

    public BuildingConnector(IBuildingContextProvider buildingContextProvider)
    {
        this.buildingContextProvider = buildingContextProvider;
    }

    public void ConnectBuildings(uint outputIndex, uint inputIndex, InputBuilding inputBuilding,
        OutputBuilding outputBuilding)
    {
        if (outputBuilding is DynamicFlowBuilding dynamicFlowsOutputBuilding)
        {
            dynamicFlowsOutputBuilding.RecomputeOutput(buildingContextProvider);
        }

        inputBuilding.InputResourceFlows[inputIndex] =
            outputBuilding.OutPutResourceFlows[outputIndex];

        if (outputBuilding is DynamicFlowBuilding otherDynamicFlowsOutputBuilding)
        {
            otherDynamicFlowsOutputBuilding.RecomputeOutput(buildingContextProvider);
        }

        if (inputBuilding is DynamicFlowBuilding dynamicFlowsInputBuilding)
        {
            dynamicFlowsInputBuilding.RecomputeOutput(buildingContextProvider);
        }
    }
}