using FiveYearPlans.ViewModels.Buildings;

namespace FiveYearPlans.ViewModels;

public interface IBuildingContextProvider
{
    public IReadOnlyDictionary<uint, Building?> GetOutputConnectionState(Guid id);
    public IReadOnlyDictionary<uint, Building?> GetInputConnectionState(Guid id);
}