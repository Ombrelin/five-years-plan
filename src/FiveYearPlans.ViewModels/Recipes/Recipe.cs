using FiveYearPlans.ViewModels.Resources;

namespace FiveYearPlans.ViewModels.Recipes;

public record Recipe(
    ResourceFlow IngredientFlow,
    ResourceFlow ProductFlow
)
{
    public override string ToString()
    {
        return $"{IngredientFlow.Resource} ({IngredientFlow.Quantity}/m) \u2192 {ProductFlow.Resource} ({ProductFlow.Quantity}/m)";
    }
}