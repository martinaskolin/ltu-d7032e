using ApplesToApples.Cards;
using ApplesToApples.Game.Phases;
using ApplesToApples.Game.Variations;
using ApplesToApples.Players;
using ApplesToApples.Resources;
using ApplesToApples.Utilities.ExtensionMethods;
using ApplesToApplesTests.TestHelpers;

namespace ApplesToApplesTests;

public class RulesTests
{
    
    private StandardGame _game;
    private PlayerManager _playerManager;
    
    [SetUp]
    public void SetUp()
    {
        
    }

    /// <summary>
    /// Tests if the List extension method Shuffle is "random" or not by
    /// analyzing the element distribution using Chi-square.
    /// (This covers rule 03, 05 and 08 since they all use this method for their randomness)
    /// </summary>
    private void ShuffleTest()
    {
        int trials = 10000;
        int size = 10;
        int[,] posCount = new int[size, size];
        List<int> original = new List<int> { 0,1,2,3,4,5,6,7,8,9 };

        for (int i = 0; i < trials; i++)
        {
            List<int> list = new List<int>(original);
            list.Shuffle();
            
            for (int newIndex = 0; newIndex < size; newIndex++)
            {
                int originalIndex = list[newIndex];
                posCount[originalIndex, newIndex]++;
            }
        }

        double expectedFreq = trials / (double)size;
        double chiSquare = 0f;

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                double observed = posCount[i, j];
                chiSquare += Math.Pow(observed - expectedFreq, 2) / expectedFreq;
            }
        }
        
        // Chi-square critical value for p = 0.05 (95% confidence)
        // For 81 degrees of freedom (df = (r-1)(c-1)): 101.879
        double criticalValue = 101.879;
        
        Assert.Less(chiSquare, criticalValue, "Shuffle does not appear to be random");
    }

    /// <summary>
    /// Tests if the Replenish phase gives cards to players until they reach 7
    /// regardless of how many they had before. (Rule 04 and 12)
    /// </summary>
    private void ReplenishTest()
    {
        // Give each player a different number of cards from 0 to 7
        for (int i = 0; i < _playerManager.Pawns.Count; i++)
        {
            for (int j = 0; j < i; j++)
            {
                _playerManager.Pawns[i].Hand.Add(new RedApple("Test"));
            }
        }
        
        // Execute phase
        ReplenishPhase phase = new ReplenishPhase(new List<IRedApple>(Resource.GetRedApples()), _playerManager.Pawns);
        phase.Execute();

        // See if each player ended up with 7 cards
        foreach (PlayerPawn pawn in _playerManager.Pawns)
        {
            Assert.That(pawn.Hand.Count, Is.EqualTo(7));
        }
    }

    [Test]
    public void Rule01Test()
    {
        List<GreenApple> greenApples = Resource.GetGreenApples();
        Assert.That(greenApples.Count, Is.EqualTo(614));
    }
    
    [Test]
    public void Rule02Test()
    {
        List<IRedApple> redApples = new List<IRedApple>(Resource.GetRedApples());
        Assert.That(redApples.Count, Is.EqualTo(1826));
    }

    [Test]
    public void Rule03Test()
    {
        ShuffleTest(); // See documentation for ShuffleTest
    }

    [Test]
    public void Rule04Test()
    {
        ReplenishTest(); // See documentation for ReplenishTest
    }

    [Test]
    public void Rule05Test()
    {
        ShuffleTest(); // See documentation for ShuffleTest
    }

    [Test]
    public void Rule06Test()
    {
        // Set up
        PlayerManager playerManager = new PlayerManager();
        for (int i = 0; i < 8; i++)
        {
            playerManager.AddPlayer(new BotController());
        }
        StandardGame game = new StandardGame(playerManager);
        
        // Set observers
        List<GreenApple> receivedGreenApples = new List<GreenApple>();
        foreach (var playerController in _playerManager.Players)
        {
            var controller = (TestController)playerController;
            controller.OnAskedToPlay += (greenApple) => { receivedGreenApples.Add(greenApple); };
        }

        while (receivedGreenApples.Count == 0)
        {
            _game.Step();
        }

        Assert.That(receivedGreenApples.Count, Is.EqualTo(playerManager.Players.Count));
        foreach (var greenApple in receivedGreenApples)
        {
            // TODO: Continue
        }
    }

    [Test]
    public void Rule08Test()
    {
        ShuffleTest(); // See documentation for ShuffleTest
    }

    [Test]
    public void Rule12Test()
    {
        ReplenishTest(); // See documentation for ReplenishTest
    }

    [Test]
    public void Rule15Test()
    {
        //StandardGame.CheckWinner()
    }
}