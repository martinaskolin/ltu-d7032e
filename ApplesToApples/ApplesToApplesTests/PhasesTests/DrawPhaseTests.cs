using ApplesToApples.Cards;
using ApplesToApples.Game.Phases;
using ApplesToApples.Resources;
using ApplesToApples.Utilities.ExtensionMethods;

namespace ApplesToApplesTests.PhasesTests;

public class DrawPhaseTests
{
    private List<GreenApple> _greenApples;
    private DrawPhase _drawPhase;
    private GreenApple value;
    
    [SetUp]
    public void SetUp()
    {
        _greenApples = Resource.GetGreenApples();
        _drawPhase = new DrawPhase(Resource.GetGreenApples());
        _drawPhase.OnDraw += i => value = i;
    }
    
    [Test]
    public void DrawTest()
    {
        while (_greenApples.Count > 0)
        {
            _drawPhase.Execute();
            Assert.That(_greenApples.RemoveTop().Adjective, Is.EqualTo(value.Adjective));
        }
    }
}