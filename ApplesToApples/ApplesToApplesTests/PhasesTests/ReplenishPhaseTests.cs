using ApplesToApples.Cards;
using ApplesToApples.Game.Phases;
using ApplesToApples.Phases;
using ApplesToApples.Players;
using ApplesToApples.Resources;

namespace ApplesToApplesTests.PhasesTests;

public class ReplenishPhaseTests
{
    private List<PlayerPawn> _pawns = new List<PlayerPawn>();
    private List<IRedApple> _redApples = new List<IRedApple>();
    private ReplenishPhase _replenishPhase;
    
    [SetUp]
    public void SetUp()
    {
        for (int i = 0; i < 3; i++)
        {
            _pawns.Add(new PlayerPawn());
        }
        _redApples = Resource.GetRedApples();
        _replenishPhase = new ReplenishPhase(_redApples, _pawns);
    }

    [Test]
    public void PileTest()
    {
        int max = _redApples.Count;
        _replenishPhase.Execute();
        Assert.That(max, Is.GreaterThan(_redApples.Count), "Cards are not being removed from pile");
    }
    
    [Test]
    public void HandTest()
    {
        _replenishPhase.Execute();
        
        foreach (PlayerPawn pawn in _pawns)
        {
            Assert.That(pawn.Hand.Count, Is.EqualTo(7), "Player does not have right amount of cards");
        }
    }
}