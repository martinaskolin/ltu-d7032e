using ApplesToApples.Cards;

namespace ApplesToApples.Players;

public class BotController : IPlayerController
{
    public PlayerPawn Pawn { get; set; }

    public Task<RedApple> Play(GreenApple greenApple)
    {
        throw new NotImplementedException();
    }

    public Task<RedApple> Judge(List<RedApple> redApples, GreenApple greenApple)
    {
        throw new NotImplementedException();
    }
}