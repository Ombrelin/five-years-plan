namespace FiveYearPlans.ViewModels.Buildings;

public abstract class Building
{
    public Guid Id { get; } = Guid.NewGuid();
}