using ApplesToApples.Cards;
using ApplesToApples.Players;

namespace ApplesToApplesTests.TestHelpers;

public class TestController : IPlayerController
{
    public PlayerPawn Pawn { get; set; }
    public int IndexToSelect = 0;
    private readonly RedApple _testRedApple = new RedApple("Testing");
    
    public Task<RedApple> Play(GreenApple greenApple)
    {
        return new Task<RedApple>(() => _testRedApple);
    }

    public Task<RedApple> Judge(List<RedApple> redApples, GreenApple greenApple)
    {
        return new Task<RedApple>(() => redApples[IndexToSelect]);
    }
}