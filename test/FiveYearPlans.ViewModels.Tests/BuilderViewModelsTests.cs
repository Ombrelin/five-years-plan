using FiveYearPlans.ViewModels.Buildings.ViewModels;
using FiveYearPlans.ViewModels.Resources;
using FiveYearPlans.ViewModels.Tests.Fakes;

namespace FiveYearPlans.ViewModels.Tests;

public class BuilderViewModelsTests
{
    private readonly BuilderViewModel target = new();
    private readonly BuildingTestHelper helper;

    public BuilderViewModelsTests()
    {
        helper = new BuildingTestHelper(target);
    }


    [Fact]
    public void Connect_NoOutputFlowConnected_AllOutputsAtZero()
    {
        // Given
        FakeBuildingContextProvider fakeBuildingContext = helper.FakeBuildingContextProviderWithTarget();

        // When
        helper.ConnectMinerToTarget(fakeBuildingContext);

        // Then
        Assert.Equal(new ResourceFlow(Resource.IronOre, 30), target.InputResourceFlows[0]);

        Assert.Equal(new ResourceFlow(Resource.IronOre, 0), target.OutPutResourceFlows[0]);
        Assert.Equal(new ResourceFlow(Resource.IronOre, 0), target.OutPutResourceFlow);
    }
}