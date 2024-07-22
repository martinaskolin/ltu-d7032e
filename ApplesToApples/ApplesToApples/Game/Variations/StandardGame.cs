using ApplesToApples.Cards;
using ApplesToApples.Game.PhaseMachines;
using ApplesToApples.Game.Phases;
using ApplesToApples.Players;
using ApplesToApples.Resources;
using ApplesToApples.Utilities.ExtensionMethods;

namespace ApplesToApples.Game.Variations;

public class StandardGame
{
    public readonly List<IRedApple> RedApples;
    public readonly List<GreenApple> GreenApples;

    private readonly IPhaseMachine _context;
    private readonly PlayerManager _playerManager;

    public StandardGame(PlayerManager playerManager)
    {
        _playerManager = playerManager;

        // Read all the green apples (adjectives) from a file and add to the green apples deck
        GreenApples = Resource.GetGreenApples();

        // Read all the red apples (nouns) from a file and add to the red apples deck
        RedApples = new(Resource.GetRedApples());

        // Shuffle both the green apples and red apples decks
        GreenApples.Shuffle();
        RedApples.Shuffle();

        _context = CreatePhases();
    }

    public void Step()
    {
        if (_context.MoveNext()) _context.Current.Execute(); // TODO: Endless loop (we dont check for winner)
    }

    public IPhaseMachine CreatePhases()
    {
        // Init phases
        ReplenishPhase replenishPhase = new ReplenishPhase(RedApples, _playerManager.Pawns);
        DrawPhase drawPhase = new DrawPhase(GreenApples);
        SubmitPhase submitPhase = new SubmitPhase(_playerManager.Players);
        JudgePhase judgePhase = new JudgePhase(_playerManager.Players);
        CheckWinnerPhase checkWinnerPhase = new CheckWinnerPhase(_playerManager.Pawns);
        
        SequentialMachine context = new SequentialMachine(new List<IPhase>()
        {
            replenishPhase,
            drawPhase,
            submitPhase,
            judgePhase
        });
        
        // Set dynamic dependencies with events (observer pattern with dependency injection)
        drawPhase.OnDraw += submitPhase.SetGreenApple;
        drawPhase.OnDraw += judgePhase.SetGreenApple;
        submitPhase.OnCardsSubmitted += judgePhase.SetSubmittedCards;
        judgePhase.OnWinnerSelected += redApple => redApple.Owner.GreenApples.Add(drawPhase.Current);
        checkWinnerPhase.OnWinnerFound += _ => context.Finished();

        return context;
    }
}