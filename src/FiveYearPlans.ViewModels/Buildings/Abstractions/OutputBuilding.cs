using FiveYearPlans.ViewModels.Resources;

namespace FiveYearPlans.ViewModels.Buildings.Abstractions;

public interface OutputBuilding : IBuilding
{
    Dictionary<uint, ResourceFlow> OutPutResourceFlows { get; }
}