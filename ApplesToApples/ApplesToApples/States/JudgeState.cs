using ApplesToApples.Utilities;

namespace ApplesToApples.States;

public class JudgeState<T> : IGameState
{
    private ISelector<T> _selector;
    private Func<List<T>> _getElements;
    private T _result;
    
    public JudgeState(Func<List<T>> getElements, ISelector<T> selector)
    {
        _selector = selector;
        _getElements = getElements;
    }
    
    public async void Execute()
    {
        _result = await _selector.SelectElement(_getElements.Invoke());
    }

    public T GetResult()
    {
        return _result;
    }
    
}