using FiveYearPlans.ViewModels.Buildings;
using FiveYearPlans.ViewModels.Buildings.Abstractions;
using FiveYearPlans.ViewModels.Resources;

namespace FiveYearPlans.ViewModels.Tests.Fakes;

public class ArbitraryProducerBuilding : Building, OutputBuilding
{
    private readonly ResourceFlow resourceFlow;

    public ArbitraryProducerBuilding(ResourceFlow resourceFlow)
    {
        this.resourceFlow = resourceFlow;
    }

    public override Dictionary<uint, ResourceFlow> OutPutResourceFlows => new()
    {
        [0] = resourceFlow
    };
}