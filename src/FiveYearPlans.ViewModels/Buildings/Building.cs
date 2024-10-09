using FiveYearPlans.ViewModels.Buildings.Abstractions;
using FiveYearPlans.ViewModels.Resources;

namespace FiveYearPlans.ViewModels.Buildings;

public abstract class Building : IBuilding
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
                var inputToUpdateIndex = GetInputToUpdateIndex(inputBuilding);
                inputBuilding.InputResourceFlows[inputToUpdateIndex] = OutPutResourceFlows[Index]; 
            }

            if (connectedOutput is DynamicFlowBuilding dynamicFlowBuilding)
            {
                dynamicFlowBuilding.RecomputeOutput(buildingContextProvider);
            }
        }
    }

    private uint GetInputToUpdateIndex(InputBuilding inputBuilding)
    {
        return this
            .buildingsProvider
            .GetInputConnectionState(inputBuilding.Id)
            .First(kvp => kvp.Value is not null && kvp.Value.Id == this.Id)
            .Key;
    }
}