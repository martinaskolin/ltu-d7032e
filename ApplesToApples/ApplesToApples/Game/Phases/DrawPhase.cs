using ApplesToApples.Cards;
using ApplesToApples.Utilities.ExtensionMethods;

namespace ApplesToApples.Game.Phases;

public class DrawPhase : IGamePhase
{
    public GreenApple Current { get; private set; }
    public event Action<GreenApple>? OnDraw;
    private readonly List<GreenApple> _drawPile;

    public DrawPhase(List<GreenApple> drawPile)
    {
        _drawPile = drawPile;
    }
    
    public async Task Execute()
    {
        Current = _drawPile.RemoveTop();
        OnDraw?.Invoke(Current);
    }
}