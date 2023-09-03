using System.Collections.ObjectModel;

namespace FiveYearPlans.ViewModels;

public static class Extensions
{
    public static void AddAll<T>(this ObservableCollection<T> collection, IEnumerable<T> other)
    {
        foreach (T element in other)
        {
            collection.Add(element);
        }
    }
}