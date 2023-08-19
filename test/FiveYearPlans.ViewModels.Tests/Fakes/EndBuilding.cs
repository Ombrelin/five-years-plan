using FiveYearPlans.ViewModels.Buildings;

namespace FiveYearPlans.ViewModels.Tests.Fakes;

public class EndBuilding : Building, InputBuilding, DynamicFlowBuilding
{
    public Dictionary<uint, ResourceFlow> InputResourceFlows { get; } = new();
    
    public ResourceFlow? RecomputedResourceFlow { get; private set; }
    
    public void RecomputeOutput(IBuildingContextProvider buildingContextProvider)
    {
        KeyValuePair<uint, ResourceFlow>? input = InputResourceFlows.SingleOrDefault();
        if (input is not null)
        {
            RecomputedResourceFlow = input.Value.Value;
        }
        
    }
}