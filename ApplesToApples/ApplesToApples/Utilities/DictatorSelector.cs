using ApplesToApples.Cards;
using ApplesToApples.Players;

namespace ApplesToApples.Utilities;

public class DictatorSelector<T> : ISelector<T>
{
    private Func<IPlayerController> _getDictator;
    public DictatorSelector(Func<IPlayerController> getDictator)
    {
        _getDictator = getDictator;
    }
    
    public Task<T> SelectElement(List<T> elements)
    {
        _getDictator.Invoke().
    }
}