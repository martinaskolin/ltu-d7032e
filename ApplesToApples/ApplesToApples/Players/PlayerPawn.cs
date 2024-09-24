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
    public event Action<IRedApple> OnCardReceived;
    public event Action<GreenApple> OnPointReceived;

    public int Points => _greenApples.Count;
    public IRedApple[] Hand => _redApples.ToArray();
    public int NumberOfCardsInHand => _redApples.Count;
    
    public readonly int Id;
    public string Name;
    
    private readonly List<GreenApple> _greenApples = new List<GreenApple>();
    private readonly List<IRedApple> _redApples = new List<IRedApple>();

    private static int _amountPlayers = 0;
    public PlayerPawn()
    {
        Id = _amountPlayers++;
        Name = $"Player {Id}";
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
    /// Tries to find and remove card from hand.
    /// </summary>
    /// <param name="card">Card to remove</param>
    /// <returns>If the card exists in the hand of the player</returns>
    public bool RemoveCard(IRedApple card)
    {
        return _redApples.Remove(card);
    }

    public IRedApple RemoveCard(int index)
    {
        return _redApples.Pop(index);
    }

}