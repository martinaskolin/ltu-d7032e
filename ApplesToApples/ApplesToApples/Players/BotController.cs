using ApplesToApples.Cards;
using ApplesToApples.Utilities.ExtensionMethods;

namespace ApplesToApples.Players;

/// <summary>
/// Controls a player pawn in Apples to Apples as an AI bot.
/// </summary>
public class BotController : IPlayerController
{
    public PlayerPawn Pawn { get;}

    public BotController(PlayerPawn pawn)
    {
        Pawn = pawn;
    }

    public Task<RedApple> Play(GreenApple greenApple)
    {
        return Task.FromResult((RedApple)Pawn.RemoveCard(0));
    }

    public Task<(IPlayerController, RedApple)> Judge(List<(IPlayerController, RedApple)> redApples, GreenApple greenApple)
    {
        return Task.FromResult(redApples[0]);
    }
}