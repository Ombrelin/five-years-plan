namespace FiveYearPlans.ViewModels;

public interface IBuildingContextProvider
{
    public IReadOnlyDictionary<uint, DynamicFlowBuilding?> GetOutputConnectionState(Guid id);
}