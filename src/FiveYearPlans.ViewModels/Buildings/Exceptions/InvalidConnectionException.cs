using FiveYearPlans.ViewModels.Resources;

namespace FiveYearPlans.ViewModels.Buildings.Exceptions;

public class InvalidConnectionException : Exception
{
    public InvalidConnectionException(Resource inputResource, Resource? recipeResource) : base($"Input resource : {inputResource} doesn't match with recipe resource : {recipeResource}")
    {
        
    }
}