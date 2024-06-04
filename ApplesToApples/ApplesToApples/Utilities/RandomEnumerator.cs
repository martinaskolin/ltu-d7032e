using System.Collections;

namespace ApplesToApples.Utilities;

/// <summary>
/// Enumerates over a enumerable randomly
/// </summary>
/// <typeparam name="T">Type to enumerate over</typeparam>
public class RandomEnumerator<T> : IEnumerator<T>
{
    private List<T> _elements;
    private List<int> _indices;
    private Random _random;
    private int _currentIndex = -1; // Standard start index for Enumerators

    public RandomEnumerator(IEnumerable<T> collection)
    {
        _elements = new List<T>(collection);
        _indices = new List<int>();
        _random = new Random();

        // Fill list with indices
        for (int i = 0; i < _elements.Count; i++)
        {
            _indices.Add(i);
        }
        
        // Shuffle list
        ShuffleIndices();
    }
    
    /// <summary>
    /// Moves to next element in enumerable.
    /// </summary>
    /// <returns>true if element exists, false otherwise</returns>
    public bool MoveNext()
    {
        _currentIndex++;
        return _currentIndex < _elements.Count;
    }

    /// <summary>
    /// Resets enumerator with new random order
    /// </summary>
    public void Reset()
    {
        _currentIndex = -1;
        ShuffleIndices();
    }

    /// <summary>
    /// Returns the current element of the enumerable
    /// </summary>
    public T Current => _elements[_indices[_currentIndex]];

    object IEnumerator.Current => Current;

    /// <summary>
    /// Not implemented.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    private void ShuffleIndices()
    {
        for (int i = 0; i < _indices.Count; i++)
        {
            int randomIndex = _random.Next(i, _indices.Count);
            (_indices[i], _indices[randomIndex]) = (_indices[randomIndex], _indices[i]);
        }
    }
}