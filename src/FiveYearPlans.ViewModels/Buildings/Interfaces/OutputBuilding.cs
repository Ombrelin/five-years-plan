namespace FiveYearPlans.ViewModels.Buildings.Interfaces;

public interface OutputBuilding : IBuilding
{
    Dictionary<uint, ResourceFlow> OutPutResourceFlows { get; }
}