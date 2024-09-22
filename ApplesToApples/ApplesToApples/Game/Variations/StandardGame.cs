using ApplesToApples.Cards;
using ApplesToApples.Game.PhaseMachines;
using ApplesToApples.Game.Phases;
using ApplesToApples.Players;
using ApplesToApples.Resources;
using ApplesToApples.Utilities.ExtensionMethods;

namespace ApplesToApples.Game.Variations;

public class StandardGame
{
    // TODO: Fix judge problem
    // TODO: Add channels for different players, and all
    // TODO: Add support for multiplayer
    
    public readonly List<IRedApple> RedApples;
    public readonly List<GreenApple> GreenApples;
    public PlayerPawn Winner;

    private readonly IPhaseMachine _context;
    private readonly List<IPlayerController> _controllers;
    private readonly List<PlayerPawn> _players;

    public StandardGame(List<IPlayerController> controllers)
    {
        _controllers = controllers;
        _players = controllers.Select(controller => controller.Pawn).ToList();

        // Read all the green apples (adjectives) from a file and add to the green apples deck
        GreenApples = Resource.GetGreenApples();

        // Read all the red apples (nouns) from a file and add to the red apples deck
        RedApples = new(Resource.GetRedApples());

        // Shuffle both the green apples and red apples decks
        GreenApples.Shuffle();
        RedApples.Shuffle();

        _context = CreatePhases();
    }

    public async Task<bool> Step()
    {
        bool hasNext = _context.MoveNext();
        if (hasNext) await _context.Current.Execute();
        
        return hasNext;
    }

    public IPhaseMachine CreatePhases()
    {
        // Init phases
        ReplenishPhase replenishPhase = new ReplenishPhase(RedApples, _players);
        DrawPhase drawPhase = new DrawPhase(GreenApples);
        JudgePhase judgePhase = new JudgePhase(_controllers);
        SubmitPhase submitPhase = new SubmitPhase(
            () => _controllers.Where(c => c != judgePhase.CurrentJudge).ToArray(),
            () => drawPhase.Current);
        CheckWinnerPhase checkWinnerPhase = new CheckWinnerPhase(_players);
        
        SequentialMachine context = new SequentialMachine(new List<IGamePhase>()
        {
            replenishPhase,
            drawPhase,
            submitPhase,
            judgePhase,
            checkWinnerPhase
        });
        
        // Set dynamic dependencies with events (observer pattern with dependency injection)
        drawPhase.OnDraw += judgePhase.SetGreenApple;
        submitPhase.OnSubmissons += judgePhase.SetSubmissions;
        checkWinnerPhase.OnWinnerFound += _ => context.Finished();
        checkWinnerPhase.OnWinnerFound += winner => Winner = winner;
        checkWinnerPhase.OnWinnerFound += winner => Console.Write($"{winner.Name} won the game\n");

        return context;
    }
}