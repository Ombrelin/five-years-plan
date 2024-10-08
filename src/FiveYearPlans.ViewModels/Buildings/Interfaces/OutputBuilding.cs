using FiveYearPlans.ViewModels.Resources;

namespace FiveYearPlans.ViewModels;

public interface OutputBuilding : IBuilding
{
    Dictionary<uint, ResourceFlow> OutPutResourceFlows { get; }
}