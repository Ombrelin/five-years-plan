using FiveYearPlans.ViewModels.Buildings;

namespace FiveYearPlans.ViewModels.Tests.Fakes;

public class FakeBuildingContextProvider : IBuildingContextProvider
{
    public IDictionary<Guid, Dictionary<uint, DynamicFlowBuilding?>> Buildings { get; } =
        new Dictionary<Guid, Dictionary<uint, DynamicFlowBuilding?>>();

    public IReadOnlyDictionary<uint, DynamicFlowBuilding?> GetOutputConnectionState(Guid id) => Buildings[id];
}