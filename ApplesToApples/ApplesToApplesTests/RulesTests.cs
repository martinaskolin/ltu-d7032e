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
    // ----------------------------------------------------------
    // Setting up the game
    // ----------------------------------------------------------
    
    [Test]
    public void Rule01Test()
    {
        // Rule 01: Read all the green apples (adjectives) from a file and add to the green apples deck.
        List<GreenApple> greenApples = Resource.GetGreenApples();
        Assert.That(greenApples.Count, Is.EqualTo(614));
    }
    
    [Test]
    public void Rule02Test()
    {
        // Rule 02: Read all the red apples (nouns) from a file and add to the red apples deck.
        List<IRedApple> redApples = new List<IRedApple>(Resource.GetRedApples());
        Assert.That(redApples.Count, Is.EqualTo(1826));
    }
    
    [Test]
    public void Rule03Test()
    {
        // Rule 03: Shuffle both the green apples and red apples decks.
        ShuffleTest();
    }
    
    [Test]
    public void Rule04Test()
    {
        // Rule 04: Deal seven red apples to each player, including the judge.
        ReplenishTest(); // See documentation for ReplenishTest
    }
    
    [Test]
    public void Rule05Test()
    {
        // Rule 05: Randomise which player starts being the judge.
        List<IPlayerController> controllers =
            new List<IPlayerController>(Enumerable.Range(0, 6).Select(_ => new TestController()).ToList());

        List<IPlayerController> judges = new List<IPlayerController>();
        for (int i = 0; i < 100; i++)
        {
            judges.Add(new JudgePhase(controllers).CurrentJudge);
        }
        
        foreach (var controller in controllers)
        {
            Assert.That(judges, Does.Contain(controller), 
                "Each player should be selected as judge at least once.");
        }

        // 2. Assert that the selection is roughly uniform
        var judgeGroups = judges.GroupBy(j => j).Select(group => new 
        {
            Judge = group.Key,
            Count = group.Count()
        }).ToList();

        // Calculate expected frequency: each player should be judge ~ (iterations / number of players)
        double expectedFrequency = 100 / (double)controllers.Count;
    
        // Chi-square test to verify randomness (similar to the shuffle test)
        double chiSquare = 0;
        foreach (var group in judgeGroups)
        {
            double observed = group.Count;
            chiSquare += Math.Pow(observed - expectedFrequency, 2) / expectedFrequency;
        }

        // Chi-square critical value for p = 0.05 (95% confidence) and df = 5 (6 players - 1)
        double criticalValue = 11.070;  // This is the critical value for df = 5 at p = 0.05

        Assert.Less(chiSquare, criticalValue, "Judge selection does not appear to be random.");
    }
    
    // ----------------------------------------------------------
    // Playing the game
    // ----------------------------------------------------------
    
    [Test]
    public void Rule06Test()
    {
        // Rule 06: A green apple is drawn from the pile and shown to everyone
        
        // Set up
        List<IPlayerController> controllers = new List<IPlayerController>();
        for (int i = 0; i < 8; i++)
        {
            controllers.Add(new TestController());
        }
        
        StandardGame game = new StandardGame(controllers);
        
        // Set observers
        List<GreenApple> receivedGreenApples = new List<GreenApple>();
        foreach (var playerController in controllers)
        {
            var controller = (TestController)playerController;
            // receivedGreenApples is incremented when a player sees the green apple.
            controller.OnAskedToPlay += (greenApple) => { receivedGreenApples.Add(greenApple); };
        }

        // Step until a green apples is shown to someone
        while (receivedGreenApples.Count == 0)
        {
            game.Step();
        }

        // Assert that it was shown to everyone
        Assert.That(receivedGreenApples.Count, Is.EqualTo(controllers.Count-1));
    }

    [Test]
    public async Task Rule07Test()
    {
        // Rule 07: All players (except the judge) choose one red apple from their hand (based on the green apple) and plays it.
        List<IPlayerController> controllers =
            new List<IPlayerController>(Enumerable.Range(0, 6).Select(_ => new TestController()).ToList());

        JudgePhase judgePhase = new JudgePhase(controllers);
        SubmitPhase submitPhase = new SubmitPhase(
            () => controllers.Where(c => c != judgePhase.CurrentJudge).ToArray(),
            () => new GreenApple("GREEN APPLE"));

        List<IPlayerController> submitters = new List<IPlayerController>();

        // When asked to play add them as a submitter
        foreach (var controller in controllers)
        {
            ((TestController)controller).OnAskedToPlay += _ => submitters.Add(controller);
        }

        List<(IPlayerController, RedApple)>? submissions = null;
        submitPhase.OnSubmissons += list => submissions = list;

        await submitPhase.Execute();
        
        var submitted = submissions.Select(tuple => tuple.Item1).ToList();
        Assert.That(submitters.Count, Is.EqualTo(submitted.Count)); // All players asked to play, played a red apple
        Assert.That(submitters, Does.Not.Contain(judgePhase.CurrentJudge)); // Except the judge
        Assert.That(submitted, Does.Not.Contain(judgePhase.CurrentJudge)); // Except the judge

    }

    [Test]
    public void Rule08Test()
    {
        ShuffleTest(); // See documentation for ShuffleTest
    }

    [Test]
    public void Rule09Test()
    {
        // Rule 09: All players (except the judge) must play their red apples before the results are shown.
        List<IPlayerController> controllers =
            new List<IPlayerController>(Enumerable.Range(0, 6).Select(_ => new TestController()).ToList());

        JudgePhase judgePhase = new JudgePhase(controllers);
        SubmitPhase submitPhase = new SubmitPhase(
            () => controllers.Where(c => c != judgePhase.CurrentJudge).ToArray(),
            () => new GreenApple("GREEN APPLE"));

        List<IPlayerController> askedToPlay = new List<IPlayerController>();
        List<IPlayerController> hasPlayed = new List<IPlayerController>();

        foreach (var controller in controllers)
        {
            ((TestController)controller).OnAskedToPlay += _ => askedToPlay.Add(controller);
        }

        submitPhase.OnSubmissons += list => hasPlayed = list.Select(tuple => tuple.Item1).ToList();

        judgePhase.OnVerdict += (winner, submission, greenApple) =>
        {
            // Everyone except for the judge was asked to play
            Assert.That(askedToPlay.Count, Is.EqualTo(controllers.Count-1));
            // Everyone asked to play a card has played a card before results are shown
            Assert.That(askedToPlay.OrderBy(x => x).SequenceEqual(hasPlayed.OrderBy(x => x)));
        };
    }

    [Test]
    public async Task Rule10Test()
    {
        // Rule 10: The judge selects a favourite red apple. The player who submitted the favourite red apple is rewarded the green apple as a point (rule 14).
        List<IPlayerController> controllers =
            new List<IPlayerController>(Enumerable.Range(0, 6).Select(_ => new TestController()).ToList());
        StandardGame game = new StandardGame(controllers);

        (IPlayerController, GreenApple)? judgesFavorite = null;
        (IPlayerController, GreenApple)? receivedAPoint = null;
        
        
        foreach (var playerController in controllers)
        {
            var controller = (TestController)playerController;
            controller.OnAskedToJudge += (winner, apple) => judgesFavorite = (winner, apple);
            controller.Pawn.OnPointReceived += apple => receivedAPoint = (controller, apple);
        }

        while (receivedAPoint == null)
        {
            await game.Step();
        }
        
        Assert.That(judgesFavorite, Is.EqualTo(receivedAPoint));
        Assert.That(receivedAPoint.Value.Item1.Pawn.Points, Is.EqualTo(1));
    }

    [Test]
    public async Task Rule11Test()
    {
        List<IPlayerController> controllers =
            new List<IPlayerController>(Enumerable.Range(0, 6).Select(_ => new BotController(new PlayerPawn())).ToList());

        foreach (IPlayerController controller in controllers)
        {
            for (int i = 0; i < 7; i++)
            {
                controller.Pawn.GiveCard(new RedApple("TEST" + i));
            }
        }

        List<(IPlayerController, RedApple)>? submissions = null;
        
        SubmitPhase phase = new SubmitPhase(() => controllers.ToArray(), () => new GreenApple(""));
        phase.OnSubmissons += list => submissions = list;

        await phase.Execute();

        foreach ((IPlayerController controller, RedApple submittedRedApple) in submissions)
        {
            Assert.That(controller.Pawn.Hand, Does.Not.Contain(submittedRedApple));
        }
    }

    [Test]
    public void Rule12Test()
    {
        ReplenishTest(); // See documentation for ReplenishTest
    }

    [Test]
    public void Rule13Test()
    {
        // Rule 13: The next player in the list becomes the judge. Repeat from step 6 until someone wins the game.
        List<IPlayerController> controllers = new List<IPlayerController>();
        while (controllers.Count < 4)
        {
            controllers.Add(new TestController());
        }
    }

    // ----------------------------------------------------------
    // Winning the game
    // ----------------------------------------------------------
    
    [Test]
    public void Rule14Test()
    {
        // Rule 14: Keep score by keeping the green apples you’ve won.
        
        PlayerPawn player = new PlayerPawn();
        for (int i = 0; i < 10; i++)
        {
            Assert.That(player.Points, Is.EqualTo(i));
            player.GivePoint(new GreenApple(""));
        }
    }

    [Test]
    public async Task Rule15Test()
    {
        // Rule 15: Here’s how to tell when the game is over:
        //              • For 4 players, 8 green apples win.
        //              • For 5 players, 7 green apples win.
        //              • For 6 players, 6 green apples win.
        //              • For 7 players, 5 green apples win.
        //              • For 8+ players, 4 green apples win.
        
        void AssertWinner(int score, int numPlayers)
        {
            switch (numPlayers)
            {
                case 4:
                    Assert.That(score, Is.EqualTo(8));
                    break;
                case 5:
                    Assert.That(score, Is.EqualTo(7));
                    break;
                case 6:
                    Assert.That(score, Is.EqualTo(6));
                    break;
                case 7:
                    Assert.That(score, Is.EqualTo(5));
                    break;
                case >=8:
                    Assert.That(score, Is.EqualTo(4));
                    break;
            }
        }
        
        for (int numPlayers = 4; numPlayers < 9; numPlayers++)
        {
            List<IPlayerController> controllers = new List<IPlayerController>();
            for (int j = 0; j < numPlayers; j++)
            {
                controllers.Add(new BotController(new PlayerPawn()));
            }
        
            StandardGame game = new StandardGame(controllers);
            while (await game.Step()) { }

            AssertWinner(game.Winner.Points, game.NumberOfPlayers);
        }
        
        
        
    }
    
    // ----------------------------------------------------------
    // Helper methods
    // ----------------------------------------------------------
    
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
        List<int> original = Enumerable.Range(0, size).ToList();

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
        // For 81 degrees of freedom (df = (r-1)(c-1)): 157.879
        double criticalValue = 157.879;
        
        Assert.Less(chiSquare, criticalValue, "Shuffle does not appear to be random");
    }
    
    /// <summary>
    /// Tests if the Replenish phase gives cards to players until they reach 7
    /// regardless of how many they had before. (Rule 04 and 12)
    /// </summary>
    private async Task ReplenishTest()
    {
        List<PlayerPawn> players = new List<PlayerPawn>();
        for (int i = 0; i < 7; i++)
        {
            players.Add(new PlayerPawn());
        }
        
        // Give each player a different number of cards from 0 to 7
        for (int i = 0; i < players.Count; i++)
        {
            for (int j = 0; j < i; j++)
            {
                players[i].GiveCard(new RedApple("Test"));
            }
        }
        
        // Execute phase
        ReplenishPhase phase = new ReplenishPhase(new List<IRedApple>(Resource.GetRedApples()), players);
        await phase.Execute();

        // See if each player ended up with 7 cards
        foreach (PlayerPawn pawn in players)
        {
            Assert.That(pawn.Hand.Count, Is.EqualTo(7));
        }
    }
}