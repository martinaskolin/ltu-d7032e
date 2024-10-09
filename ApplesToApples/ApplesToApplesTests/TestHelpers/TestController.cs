using ApplesToApples.Cards;
using ApplesToApples.Players;

namespace ApplesToApplesTests.TestHelpers;

/// <summary>
/// A test controller for testing the game.
/// </summary>
public class TestController : IPlayerController
{
    /// <summary>
    /// Invoked when the player is asked to play a card.
    /// </summary>
    public event Action<GreenApple> OnAskedToPlay;
    /// <summary>
    /// Invoked when the player plays a card. contains the card played.
    /// </summary>
    public event Action<RedApple> OnCardPlayed;
    /// <summary>
    /// Invoked when the player is asked to judge a round.
    /// </summary>
    public event Action<IPlayerController, GreenApple> OnAskedToJudge;
    public PlayerPawn Pawn { get; set; }
    public int IndexToSelect = 0;
    private readonly RedApple _testRedApple = new RedApple("Testing");

    public TestController()
    {
        Pawn = new PlayerPawn();
    }
    
    public Task<RedApple> Play(GreenApple greenApple)
    {
        OnAskedToPlay?.Invoke(greenApple);
        OnCardPlayed?.Invoke(_testRedApple);
        return Task.FromResult(_testRedApple);
    }

    public Task<(IPlayerController, RedApple)> Judge(List<(IPlayerController, RedApple)> submissions, GreenApple greenApple)
    {
        (IPlayerController, RedApple) verdict = submissions[IndexToSelect % submissions.Count];
        OnAskedToJudge?.Invoke(verdict.Item1, greenApple);
        return Task.FromResult(verdict);
    }
}