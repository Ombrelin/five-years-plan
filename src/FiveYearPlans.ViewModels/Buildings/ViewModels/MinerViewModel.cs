using CommunityToolkit.Mvvm.ComponentModel;

namespace FiveYearPlans.ViewModels.Buildings.ViewModels;

[ObservableObject]
public partial class MinerViewModel : Building, OutputBuilding
{
    [ObservableProperty] private ResourceFlow outPutResourceFlow;
    
    public MinerViewModel()
    {
        OutPutResourceFlow = new ResourceFlow(new Resource("Iron Ore"), 30);
    }
    
    public IReadOnlyDictionary<uint, ResourceFlow> OutPutResourceFlows => new Dictionary<uint, ResourceFlow>
    {
        [0] = outPutResourceFlow
    };
}