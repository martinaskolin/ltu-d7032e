using ApplesToApples.Cards;

namespace ApplesToApples.Players;

/// <summary>
/// Defines methods and properties for controlling a player pawn in Apples to Apples.
/// Implementations include control for human players, AI bots, and testing scenarios.
/// </summary>
public interface IPlayerController
{
    /// <summary>
    /// Pawn controlled by this controller.
    /// </summary>
    public PlayerPawn Pawn { get; }
    
    /// <summary>
    /// Plays a red apple card in response to a green apple card.
    /// </summary>
    public Task<RedApple> Play(GreenApple greenApple);
    
    /// <summary>
    /// Chooses a winning red apple card from a list of submissions.
    /// </summary>
    public Task<(IPlayerController, RedApple)> Judge(List<(IPlayerController, RedApple)> submissions, GreenApple greenApple);
}