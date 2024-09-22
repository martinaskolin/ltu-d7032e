using ApplesToApples.Cards;
using ApplesToApples.Players;

namespace ApplesToApplesTests.TestHelpers;

public class TestController : IPlayerController
{
    public event Action<GreenApple> OnAskedToPlay;
    public PlayerPawn Pawn { get; set; }
    public int IndexToSelect = 0;
    private readonly RedApple _testRedApple = new RedApple("Testing");
    
    public Task<RedApple> Play(GreenApple greenApple)
    {
        OnAskedToPlay?.Invoke(greenApple);
        return new Task<RedApple>(() => _testRedApple);
    }

    public Task<(IPlayerController, RedApple)> Judge(List<(IPlayerController, RedApple)> submissions, GreenApple greenApple)
    {
        return new Task<(IPlayerController, RedApple)>(() => submissions[IndexToSelect % submissions.Count]);
    }
}