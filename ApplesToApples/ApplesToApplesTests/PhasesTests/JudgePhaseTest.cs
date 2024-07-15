using ApplesToApples.Cards;
using ApplesToApples.Game.Phases;
using ApplesToApples.Players;
using ApplesToApples.Resources;
using ApplesToApplesTests.TestHelpers;

namespace ApplesToApplesTests.PhasesTests;

public class JudgePhaseTest
{
    private JudgePhase _judgePhase;
    private TestController _controller;
    private List<RedApple> _cards;

    [SetUp]
    public void SetUp()
    {
        _cards = Resource.GetRedApples();
        _controller = new TestController();
        _judgePhase = new JudgePhase(new List<IPlayerController>{_controller});
        _judgePhase.SetGreenApple(new GreenApple("Testing"));
        _judgePhase.SetSubmittedCards(_cards);
    }

    [Test]
    public void WinnerTest()
    {
        RedApple? temp = null;
        _judgePhase.OnWinnerSelected += (winner) =>
        {
            if (temp != null) CheckWinner(winner, temp);
            else throw new Exception("Failed to initialize expected result in test");
        };
        
        foreach (var item in _cards.Select((value, index) => new {value, index}))
        {
            _controller.IndexToSelect = item.index;
            temp = item.value;
            _judgePhase.Execute();
        }

        void CheckWinner(RedApple winner, RedApple? expected)
        {
            Assert.That(winner, Is.EqualTo(expected), "Actual winner differed from selected winner");
        }
        
    }
}