using ApplesToApples.Cards;
using ApplesToApples.Phases;
using ApplesToApples.Utilities.ExtensionMethods;

namespace ApplesToApples.Game.Phases;

public class DrawPhase : IPhase
{
    public GreenApple Current { get; private set; }
    public event Action<GreenApple>? OnDraw;
    private readonly List<GreenApple> _drawPile;

    public DrawPhase(List<GreenApple> drawPile)
    {
        _drawPile = drawPile;
    }
    
    public void Execute()
    {
        Current = _drawPile.RemoveTop();
        OnDraw?.Invoke(Current);
    }
}