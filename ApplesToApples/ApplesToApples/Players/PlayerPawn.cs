using System.Data.Common;
using ApplesToApples.Cards;

namespace ApplesToApples.Players;

public class PlayerPawn
{
    private static int _amountPlayers = 0;
    
    public readonly List<GreenApple> GreenApples = new List<GreenApple>();
    public readonly List<IRedApple> Hand = new List<IRedApple>();

    public readonly int Id;
    public string Name;

    public PlayerPawn()
    {
        Id = _amountPlayers++;
        Name = $"Player {Id}";
    }

    public void GiveGreenApple(GreenApple apple)
    {
        GreenApples.Add(apple);
    }

    public void GiveRedApple(IRedApple apple)
    {
        apple.Owner = this;
        Hand.Add(apple);
    }
}