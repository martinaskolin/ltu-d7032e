namespace ApplesToApples.Utilities.ExtensionMethods;

public static class ListExtensions
{
    private static readonly Random Rng = new Random();
    
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    /// <summary>
    /// Removes and returns the item at index
    /// </summary>
    public static T Pop<T>(this IList<T> list, int index)
    {
        T item = list[index];
        list.RemoveAt(index);
        return item;
    }

    /// <summary>
    /// Removes and returns the last item in the list
    /// </summary>
    public static T RemoveTop<T>(this IList<T> list)
    {
        return list.Pop(list.Count - 1);
    }
}