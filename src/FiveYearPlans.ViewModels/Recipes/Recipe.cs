namespace FiveYearPlans.ViewModels.Recipes;

public record Recipe(string Name, IReadOnlyCollection<ResourceFlow> Ingredients,
    IReadOnlyCollection<ResourceFlow> Products);