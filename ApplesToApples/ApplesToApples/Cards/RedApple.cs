using ApplesToApples.Players;

namespace ApplesToApples.Cards;

public class RedApple : IRedApple
{

    private readonly string _noun;

    public RedApple(string noun)
    {
        _noun = noun;
    }

    public PlayerPawn Owner { get; set; }

    public void PlayCard()
    {
        throw new NotImplementedException();
    }
}