namespace FiveYearPlans.ViewModels;


public interface OutputBuilding
{
    IReadOnlyDictionary<uint, ResourceFlow> OutPutResourceFlows { get;  }
}