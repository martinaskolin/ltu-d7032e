using System.Data.Common;
using ApplesToApples.Cards;

namespace ApplesToApples.Players;

public class PlayerPawn
{
    private static int _amountPlayers = 0;
    
    public List<GreenApple> GreenApples = new List<GreenApple>();
    public List<IRedApple> Hand = new List<IRedApple>();

    public readonly int Id;
    public string Name;

    public PlayerPawn()
    {
        Id = _amountPlayers++;
        Name = $"Player {Id}";
    }
}