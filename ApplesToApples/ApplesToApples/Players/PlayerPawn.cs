using ApplesToApples.Cards;

namespace ApplesToApples.Players;

public class PlayerPawn
{
    private IPlayerController _controller;
    private List<IRedApple> _redApples = new List<IRedApple>();
    
    public void GiveRedApple(IRedApple redApple)
    {
        _redApples.Add(redApple);
    }
    
}