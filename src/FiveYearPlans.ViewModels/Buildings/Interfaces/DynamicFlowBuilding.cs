namespace FiveYearPlans.ViewModels;

public interface DynamicFlowBuilding : InputBuilding
{
    void RecomputeOutput(IBuildingContextProvider buildingContextProvider);
}