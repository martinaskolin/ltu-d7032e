using ApplesToApples.Cards;
using ApplesToApples.Game.Phases;
using ApplesToApples.Game.Variations;
using ApplesToApples.Players;
using ApplesToApples.Resources;
using ApplesToApples.Utilities.ExtensionMethods;
using ApplesToApplesTests.TestHelpers;

namespace ApplesToApplesTests;

/// <summary>
/// Tests for the rules of Apples to Apples.
/// </summary>
public class RulesTests
{
    // ----------------------------------------------------------
    // Setting up the game
    // ----------------------------------------------------------
    
    /// <summary>
    /// Rule 01: Read all the green apples (adjectives) from a file and add to the green apples deck.
    /// </summary>
    [Test] public void Rule01Test()
    {
        List<GreenApple> greenApples = Resource.GetGreenApples();
        Assert.That(greenApples.Count, Is.EqualTo(614));
    }
    
    /// <summary>
    /// Rule 02: Read all the red apples (nouns) from a file and add to the red apples deck.
    /// </summary>
    [Test] public void Rule02Test()
    {
        List<IRedApple> redApples = new List<IRedApple>(Resource.GetRedApples());
        Assert.That(redApples.Count, Is.EqualTo(1826));
    }
    
    /// <summary>
    /// Rule 03: Shuffle both the green apples and red apples decks.
    /// </summary>
    [Test] public void Rule03Test()
    {
        List<GreenApple> originalGreenApples = Resource.GetGreenApples();
        List<IRedApple> originalRedApples = new List<IRedApple>(Resource.GetRedApples());
        
        List<GreenApple> shuffledGreenApples = Resource.GetGreenApples();
        List<IRedApple> shuffledRedApples = new List<IRedApple>(Resource.GetRedApples());
        
        shuffledGreenApples.Shuffle();
        shuffledRedApples.Shuffle();
        
        // Check if the order is different from the originals
        Assert.IsFalse(Enumerable.SequenceEqual(originalGreenApples, shuffledGreenApples)); 
        Assert.IsFalse(Enumerable.SequenceEqual(originalRedApples, shuffledRedApples)); 
    }
    
    /// <summary>
    /// Rule 04: Deal seven red apples to each player, including the judge.
    /// </summary>
    [Test] public Task Rule04Test()
    {
        return ReplenishTest(); // See documentation for ReplenishTest
    }
    
    /// <summary>
    /// Rule 05: Randomise which player starts being the judge.
    /// </summary>
    [Test] public void Rule05Test()
    {
        List<IPlayerController> controllers =
            new List<IPlayerController>(Enumerable.Range(0, 6).Select(_ => new TestController()).ToList());

        // Create a list of which player starts being the judge for 100 instances
        List<IPlayerController> judges = new List<IPlayerController>();
        for (int i = 0; i < 100; i++)
        {
            JudgePhase phase = new JudgePhase(controllers);
            phase.Initialize();
            judges.Add(phase.CurrentJudge);
        }
        
        // Ensure that each player has been selected as judge at least once
        foreach (var controller in controllers)
        {
            Assert.That(judges, Does.Contain(controller), 
                "Each player should be selected as judge at least once.");
        }
    }
    
    // ----------------------------------------------------------
    // Playing the game
    // ----------------------------------------------------------
    
    /// <summary>
    /// Rule 06: A green apple is drawn from the pile and shown to everyone
    /// </summary>
    [Test] public void Rule06Test()
    {
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

    /// <summary>
    /// Rule 07: All players (except the judge) choose one red apple from their hand (based on the green apple) and plays it.
    /// </summary>
    [Test] public async Task Rule07Test()
    {
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

    /// <summary>
    /// Rule 08: The printed order of the played red apples should be randomised before shown to everyone.
    /// </summary>
    [Test] public async Task Rule08Test()
    {
        List<IPlayerController> controllers =
            new List<IPlayerController>(Enumerable.Range(0, 100).Select(_ => new TestController()).ToList());

        JudgePhase judgePhase = new JudgePhase(controllers);
        SubmitPhase submitPhase = new SubmitPhase(
            () => controllers.Where(c => c != judgePhase.CurrentJudge).ToArray(),
            () => new GreenApple("GREEN APPLE"));

        var cardsInSubmittedOrder = new List<RedApple>();

        // When asked to play add them as a submitter
        foreach (var controller in controllers)
        {
            ((TestController)controller).OnCardPlayed += card => cardsInSubmittedOrder.Add(card);
        }

        List<(IPlayerController, RedApple)>? submissions = null;
        submitPhase.OnSubmissons += list => submissions = list;

        await submitPhase.Execute();
        
        var cardsInPrintedOrder = submissions.Select(tuple => tuple.Item2).ToList();
        
        // Cards in printed order are not equal to cards in submitted order
        Assert.IsFalse(Enumerable.SequenceEqual(cardsInSubmittedOrder, cardsInPrintedOrder)); 
    }

    /// <summary>
    /// Rule 09: All players (except the judge) must play their red apples before the results are shown.
    /// </summary>
    [Test] public void Rule09Test()
    {
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

    /// <summary>
    /// Rule 10: The judge selects a favourite red apple. The player who submitted the favourite red apple is rewarded the green apple as a point (rule 14).
    /// </summary>
    [Test] public async Task Rule10Test()
    {
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

    /// <summary>
    /// Rule 11: All the submitted red apples are discarded
    /// </summary>
    [Test] public async Task Rule11Test()
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

    /// <summary>
    /// Rule 12: All players are given new red apples until they have 7 red apples
    /// </summary>
    [Test] public Task Rule12Test()
    {
        return ReplenishTest(); // See documentation for ReplenishTest
    }

    /// <summary>
    /// Rule 13: The next player in the list becomes the judge. Repeat from step 6 until someone wins the game.
    /// </summary>
    [Test] public async Task Rule13Test()
    {
        List<IPlayerController> controllers = new List<IPlayerController>();
        while (controllers.Count < 4)
        {
            controllers.Add(new TestController());
        }

        StandardGame game = new StandardGame(controllers);

        int? index = null;
        game.OnNewJudge += controller =>
        {
            int i = controllers.IndexOf(controller);
            if (index == null) index = i; // Set the first index of the judge
            else
            {
                index = (index+1)%game.NumberOfPlayers;
                Assert.That(i, Is.EqualTo(index)); // Assert that the next player in the list becomes the judge
            }
        };
        
        bool done = false;
        while (!done)
        {
            done = !await game.Step();
        }
    }

    // ----------------------------------------------------------
    // Winning the game
    // ----------------------------------------------------------
    
    /// <summary>
    /// Rule 14: Keep score by keeping the green apples you’ve won.
    /// </summary>
    [Test] public void Rule14Test()
    {
        
        PlayerPawn player = new PlayerPawn();
        for (int i = 0; i < 10; i++)
        {
            Assert.That(player.Points, Is.EqualTo(i));
            player.GivePoint(new GreenApple(""));
        }
    }

    /// <summary>
    /// Rule 15: Here’s how to tell when the game is over:
    ///              • For 4 players, 8 green apples win.
    ///              • For 5 players, 7 green apples win.
    ///              • For 6 players, 6 green apples win.
    ///              • For 7 players, 5 green apples win.
    ///              • For 8+ players, 4 green apples win.
    /// </summary>
    [Test] public async Task Rule15Test()
    {
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
    /// Tests if the Replenish phase gives cards to players until they reach 7
    /// regardless of how many they had before. (Rule 04 and 12)
    /// </summary>
    private async Task ReplenishTest()
    {
        // Init players
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