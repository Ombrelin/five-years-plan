using FiveYearPlans.ViewModels.Resources;

namespace FiveYearPlans.ViewModels.Buildings.Abstractions;

public interface InputBuilding : IBuilding
{
    Dictionary<uint, ResourceFlow> InputResourceFlows { get; }
}