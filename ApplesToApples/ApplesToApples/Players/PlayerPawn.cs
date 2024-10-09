using System.Data.Common;
using ApplesToApples.Cards;
using ApplesToApples.Utilities.ExtensionMethods;

namespace ApplesToApples.Players;

/// <summary>
/// Represents the player pawn in the game, containing all core player logic
/// such as cards and points. The <see cref="PlayerPawn"/> is manipulated
/// by controllers implementing the <see cref="IPlayerController"/> interface.
/// </summary>
public class PlayerPawn
{
    
    /// <summary>
    /// Event that is triggered when a player receives a new card.
    /// </summary>
    public event Action<IRedApple> OnCardReceived;
    /// <summary>
    /// Event that is triggered when the player receives a point.
    /// </summary>
    public event Action<GreenApple> OnPointReceived;

    /// <summary>
    /// Current points the player has. Calculated by the amount of green apples the player has.
    /// </summary>
    public int Points => _greenApples.Count;
    /// <summary>
    /// The hand of the player, containing all red apples the player has.
    /// </summary>
    public IRedApple[] Hand => _redApples.ToArray();
    public int NumberOfCardsInHand => _redApples.Count;

    public string Name;
    
    private readonly List<GreenApple> _greenApples = new List<GreenApple>();
    private readonly List<IRedApple> _redApples = new List<IRedApple>();

    private static int _amountPlayers = 0;
    public PlayerPawn()
    {
        var id = _amountPlayers++;
        Name = $"Player {id}";
    }

    public void GivePoint(GreenApple greenApple)
    {
        _greenApples.Add(greenApple);
        OnPointReceived?.Invoke(greenApple);
    }
    
    public void GiveCard(IRedApple redApple)
    {
        _redApples.Add(redApple);
        OnCardReceived?.Invoke(redApple);
    }

    /// <summary>
    /// Removes a card from the player's hand at the specified index.
    /// </summary>
    public IRedApple RemoveCard(int index)
    {
        return _redApples.Pop(index);
    }

}