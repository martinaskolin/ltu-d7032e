using ApplesToApples.Cards;
using ApplesToApples.Players;

namespace ApplesToApplesTests.TestHelpers;

public class TestController : IPlayerController
{
    public event Action<GreenApple> OnAskedToPlay;
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
        return Task.FromResult(_testRedApple);
    }

    public Task<(IPlayerController, RedApple)> Judge(List<(IPlayerController, RedApple)> submissions, GreenApple greenApple)
    {
        (IPlayerController, RedApple) verdict = submissions[IndexToSelect % submissions.Count];
        OnAskedToJudge?.Invoke(verdict.Item1, greenApple);
        return Task.FromResult(verdict);
    }
}