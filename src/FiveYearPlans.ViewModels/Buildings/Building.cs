using FiveYearPlans.ViewModels.Buildings.Interfaces;

namespace FiveYearPlans.ViewModels.Buildings;

public abstract class Building
{
    public IBuildingContextProvider? buildingsProvider;

    public Guid Id { get; } = Guid.NewGuid();

    public abstract Dictionary<uint, ResourceFlow> OutPutResourceFlows { get; }

    public void RecomputeChildren(IBuildingContextProvider buildingContextProvider,
        KeyValuePair<uint, Building?>[] connectedOutputs)
    {
        this.buildingsProvider = buildingContextProvider;
        foreach ((uint Index, Building connectedOutput) in connectedOutputs)
        {
            if (connectedOutput is InputBuilding inputBuilding)
            {
                inputBuilding.InputResourceFlows[0] = OutPutResourceFlows[Index]; // TODO: Handle multi input buildings
            }

            if (connectedOutput is DynamicFlowBuilding dynamicFlowBuilding)
            {
                dynamicFlowBuilding.RecomputeOutput(buildingContextProvider);
            }
        }
    }
}