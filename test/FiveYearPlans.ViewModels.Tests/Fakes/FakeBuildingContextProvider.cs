using FiveYearPlans.ViewModels.Buildings;

namespace FiveYearPlans.ViewModels.Tests.Fakes;

public class FakeBuildingContextProvider : IBuildingContextProvider
{
    public IDictionary<Guid, Dictionary<uint, Building?>> OutputBuildings { get; } =
        new Dictionary<Guid, Dictionary<uint, Building?>>();
    
    public IDictionary<Guid, Dictionary<uint, Building?>> InputBuildings { get; } =
        new Dictionary<Guid, Dictionary<uint, Building?>>();

    public IReadOnlyDictionary<uint, Building?> GetOutputConnectionState(Guid id) => OutputBuildings[id];
    public IReadOnlyDictionary<uint, Building?> GetInputConnectionState(Guid id) => InputBuildings[id];
}