using ApplesToApples.Players;

namespace ApplesToApples.Cards;

public class RedApple : IRedApple
{
    private string _noun;

    public RedApple(string noun)
    {
        _noun = noun;
    }
    
    public void PlayCard(PlayerPawn player)
    {
        throw new NotImplementedException();
    }
}