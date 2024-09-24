using ApplesToApples.Cards;

namespace ApplesToApples.Players;

/// <summary>
/// Defines methods and properties for controlling a player pawn in Apples to Apples.
/// Implementations include control for human players, AI bots, and testing scenarios.
/// </summary>
public interface IPlayerController
{
    public PlayerPawn Pawn { get; }
    public Task<RedApple> Play(GreenApple greenApple);
    public Task<(IPlayerController, RedApple)> Judge(List<(IPlayerController, RedApple)> submissions, GreenApple greenApple);
}