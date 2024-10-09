using System.Security.Cryptography;

namespace ApplesToApples.Utilities.ExtensionMethods;

public static class ListExtensions
{
    /// <summary>
    /// Shuffles the list in place like a deck of cards
    /// </summary>
    public static IList<T> Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = GetRandomNumber(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }

        return list;
        
        int GetRandomNumber(int maxExclusive)
        {
            byte[] randomNumber = new byte[4];
            RandomNumberGenerator.Fill(randomNumber);
            return Math.Abs(BitConverter.ToInt32(randomNumber, 0)) % maxExclusive;
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
    /// Removes and returns the last item in the list like a stack of cards
    /// </summary>
    public static T RemoveTop<T>(this IList<T> list)
    {
        return list.Pop(list.Count - 1);
    }
}