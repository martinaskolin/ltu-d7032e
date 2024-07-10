using ApplesToApples.Players;

namespace ApplesToApples.Cards;

public interface IRedApple
{
    public PlayerPawn Owner { get; set; }
    public void PlayCard();
}