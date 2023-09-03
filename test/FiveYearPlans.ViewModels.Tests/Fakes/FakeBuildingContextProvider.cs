using FiveYearPlans.ViewModels.Buildings;

namespace FiveYearPlans.ViewModels.Tests.Fakes;

public class FakeBuildingContextProvider : IBuildingContextProvider
{
    public IDictionary<Guid, Dictionary<uint, Building?>> Buildings { get; } =
        new Dictionary<Guid, Dictionary<uint, Building?>>();

    public IReadOnlyDictionary<uint, Building?> GetOutputConnectionState(Guid id) => Buildings[id];
}