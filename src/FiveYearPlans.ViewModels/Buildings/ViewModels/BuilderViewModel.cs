using CommunityToolkit.Mvvm.ComponentModel;

namespace FiveYearPlans.ViewModels.Buildings.ViewModels;

[ObservableObject]
public partial class BuilderViewModel : Building, OutputBuilding, DynamicFlowBuilding
{
    [ObservableProperty] private ResourceFlow outPutResourceFlow;
    [ObservableProperty] private ResourceFlow inputResourceFlow;
    
    private readonly Dictionary<uint, ResourceFlow> outPutResourceFlows = new ();

    public IReadOnlyDictionary<uint, ResourceFlow> OutPutResourceFlows => outPutResourceFlows;

    public Dictionary<uint, ResourceFlow> InputResourceFlows { get; } = new();

    public void RecomputeOutput(IBuildingContextProvider buildingContextProvider)
    {
        if (InputResourceFlows.Any())
        {
            InputResourceFlow = InputResourceFlows.Single().Value;
            outPutResourceFlows[0] = InputResourceFlow;
            OutPutResourceFlow = InputResourceFlow;
        }
    }
}