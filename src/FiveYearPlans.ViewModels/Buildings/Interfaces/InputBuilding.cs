using FiveYearPlans.ViewModels.Resources;

namespace FiveYearPlans.ViewModels;

public interface InputBuilding : IBuilding
{
    Dictionary<uint, ResourceFlow> InputResourceFlows { get; }
}