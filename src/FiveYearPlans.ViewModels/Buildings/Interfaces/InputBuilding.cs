using FiveYearPlans.ViewModels.Resources;

namespace FiveYearPlans.ViewModels.Buildings.Interfaces;

public interface InputBuilding : IBuilding
{
    Dictionary<uint, ResourceFlow> InputResourceFlows { get; }
}