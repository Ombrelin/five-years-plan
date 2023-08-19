using FiveYearPlans.ViewModels.Buildings.ViewModels;

namespace FiveYearPlans.ViewModels.Tests;

public class BuilderViewModelsTests
{
    private readonly BuilderViewModel target = new();

    /*
    [Fact]
    public void RecomputeResourceFlowsFromIO_UpdatesResourceFlows()
    {
        // Given
        target.InputResourceFlows[0] = new ResourceFlow(new Resource("Iron Ore"), 30);

        // When
        target.RecomputeOutput(new Dictionary<uint, bool>());

        // Then
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 30), target.InputResourceFlow);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 30), target.OutPutResourceFlow);
        Assert.Equal(new ResourceFlow(new Resource("Iron Ore"), 30), target.OutPutResourceFlows[0]);
    }

    [Fact]
    public void RecomputeResourceFlowsFromIO_MoreThanOneInputThrows()
    {
      
        // Given
        target.InputResourceFlows[0] = new ResourceFlow(new Resource("Iron Ore"), 30);
        target.InputResourceFlows[1] = new ResourceFlow(new Resource("Iron Ore"), 30);

        // When
        var act = () => target.RecomputeOutput(new Dictionary<uint, bool>());

        // Then
        Assert.Throws<InvalidOperationException>(act);
    }
    */
}