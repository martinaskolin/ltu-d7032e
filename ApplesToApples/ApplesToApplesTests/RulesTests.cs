using ApplesToApples.Cards;
using ApplesToApples.Game.Variations;
using ApplesToApples.Players;
using ApplesToApples.Resources;

namespace ApplesToApplesTests;

public class RulesTests
{
    private StandardGame _game;
    private PlayerManager _playerManager;
    
    [SetUp]
    public void SetUp()
    {
        _playerManager = new PlayerManager();
        for (int i = 0; i < 4; i++)
        {
            _playerManager.AddPlayer(new BotController());
        }
        
        _game = new StandardGame(_playerManager);
    }

    [Test]
    public void Rule01Test()
    {
        foreach (var greenApple in Resource.GetGreenApples())
        {
            Assert.That(_game.GreenApples.Contains(greenApple), "Green Apple was missing from pile");
        }
    }
    
    [Test]
    public void Rule02Test()
    {
        foreach (var redApple in Resource.GetRedApples())
        {
            Assert.That(_game.RedApples.Contains(redApple), "Red Apple was missing from pile");
        }
    }
    
    [Test]
    public void Rule03Test()
    {
        int indexChanged = 0;
        foreach (var item in Resource.GetGreenApples().Select((value, index) => new {value, index}))
        {
            if (!Equals(item.value, _game.GreenApples[item.index])) indexChanged++;
        }

        float percentChanged = float.Round( (float) indexChanged / _game.GreenApples.Count, 3);
        Assert.That(percentChanged, Is.GreaterThanOrEqualTo(0.2f), "Less than 20% of Green Apples have switched place");
        
        indexChanged = 0;
        foreach (var item in Resource.GetRedApples().Select((value, index) => new {value, index}))
        {
            if (!Equals(item.value, _game.RedApples[item.index])) indexChanged++;
        }

        percentChanged = (float) indexChanged / _game.RedApples.Count;
        Assert.That(percentChanged, Is.GreaterThanOrEqualTo(0.2f), "Less than 20% of Red Apples have switched place");
    }

    [Test]
    public void Rule04Test()
    {
        _game.Step(); // Replenish phase
        foreach (PlayerPawn pawn in _playerManager.Pawns)
        {
            Assert.That(pawn.Hand.Count, Is.EqualTo(7), $"Player hand had {pawn.Hand.Count} cards instead of 7");
        }
    }

    [Test]
    public void Rule15Test()
    {
        //StandardGame.CheckWinner()
    }
}