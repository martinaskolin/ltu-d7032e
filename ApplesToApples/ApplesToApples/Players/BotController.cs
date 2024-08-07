using ApplesToApples.Cards;

namespace ApplesToApples.Players;

public class BotController : IPlayerController
{
    public PlayerPawn Pawn { get; set; }

    public Task<RedApple> Play(GreenApple greenApple)
    {
        return Task.FromResult((RedApple)Pawn.Hand[0]); // TODO: change cast before new game variations
    }

    public Task<RedApple> Judge(List<RedApple> redApples, GreenApple greenApple)
    {
        return Task.FromResult(redApples[0]);
    }
}