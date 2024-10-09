using ApplesToApples.Cards;
using ApplesToApples.Utilities.ExtensionMethods;

namespace ApplesToApples.Game.Phases;

/// <summary>
/// Phase where a card is drawn from the draw pile.
/// </summary>
public class DrawPhase : IGamePhase
{
    /// <summary>
    /// Invoked when a card is drawn.
    /// </summary>
    public event Action<GreenApple>? OnDraw;
    
    public GreenApple Current { get; private set; }
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