namespace ApplesToApples.States;

/// <summary>
/// State responsible for drawing elements of type T from given enumerator.
/// </summary>
/// <typeparam name="T">In context of ApplesToApples the generic should be either GreenApple or IRedApple.</typeparam>
public class DrawState<T> : IGameState
{
    private IEnumerator<T> _enumerator;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="enumerator">Enumerator used for drawing items from the list (eg. top-down, bottom-up, random, etc.)</param>
    public DrawState(IEnumerator<T> enumerator)
    {
        _enumerator = enumerator;
        
    }
    
    public void Execute()
    {
        bool available = _enumerator.MoveNext();
        
        // If there are no cards left we reset the deck
        if (!available)
        {
            _enumerator.Reset();
            _enumerator.MoveNext();
        }
        
    }

    public T GetCurrent()
    {
        return _enumerator.Current;
    }

    
}