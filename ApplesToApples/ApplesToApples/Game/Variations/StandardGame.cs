using ApplesToApples.Cards;
using ApplesToApples.Game.PhaseMachines;
using ApplesToApples.Game.Phases;
using ApplesToApples.Players;
using ApplesToApples.Resources;
using ApplesToApples.Utilities;
using ApplesToApples.Utilities.ExtensionMethods;

namespace ApplesToApples.Game.Variations;

/// <summary>
/// Contains the logic for the standard game variation.
/// </summary>
public class StandardGame
{
    /// <summary>
    /// Winner of the game. Null if no winner has been found yet.
    /// </summary>
    public PlayerPawn? Winner { get; private set; }
    public int NumberOfPlayers => _players.Count;
    public event Action<IPlayerController> OnNewJudge;

    private readonly IPhaseMachine _context;
    private readonly List<IPlayerController> _controllers;
    private readonly List<PlayerPawn> _players;
    private readonly List<IRedApple> _redApples;
    private readonly List<GreenApple> _greenApples;
    private JudgePhase _judgePhase;

    public StandardGame(List<IPlayerController> controllers)
    {
        _controllers = controllers;
        _players = controllers.Select(controller => controller.Pawn).ToList();

        // Read all the green apples (adjectives) from a file and add to the green apples deck
        _greenApples = Resource.GetGreenApples();

        // Read all the red apples (nouns) from a file and add to the red apples deck
        _redApples = new(Resource.GetRedApples());
        
        // Shuffle both the green apples and red apples decks
        _greenApples.Shuffle();
        _redApples.Shuffle();

        _context = CreatePhases();

    }

    /// <summary>
    /// Steps through the game phases.
    /// </summary>
    /// <returns>True if there are more phases to execute</returns>
    public async Task<bool> Step()
    {
        bool hasNext = _context.MoveNext();
        if (hasNext) await _context.Current.Execute();
        
        return hasNext;
    }
    
    private IPhaseMachine CreatePhases()
    {
        // Init phases
        ReplenishPhase replenishPhase = new ReplenishPhase(_redApples, _players);
        DrawPhase drawPhase = new DrawPhase(_greenApples); 
        _judgePhase = new JudgePhase(_controllers);
        SubmitPhase submitPhase = new SubmitPhase(
            () => _controllers.Where(c => c != _judgePhase.CurrentJudge).ToArray(),
            () => drawPhase.Current);
        CheckWinnerPhase checkWinnerPhase = new CheckWinnerPhase(_players, IsWinner);
        
        SequentialMachine context = new SequentialMachine(new List<IPhase>()
        {
            replenishPhase,
            drawPhase,
            submitPhase,
            _judgePhase,
            checkWinnerPhase
        });
        
        // Set dynamic dependencies with events (observer pattern with dependency injection)
        context.OnListReset += () =>
        {
            PlayerNotificationSystem.Broadcast(TextFormatter.Title("NEW ROUND"), Channel.Submitters);
            PlayerNotificationSystem.Broadcast(TextFormatter.Title("JUDGE - NEW ROUND"), Channel.Judge);
        };
        drawPhase.OnDraw += apple => PlayerNotificationSystem.Broadcast("Green Apple: " + apple, Channel.All);
        drawPhase.OnDraw += _judgePhase.SetGreenApple;
        submitPhase.OnSubmissons += _judgePhase.SetSubmissions;
        _judgePhase.OnVerdict += (winner, _, greenApple) => winner.Pawn.GivePoint(greenApple);
        _judgePhase.OnVerdict += (winner, _, _) =>
            PlayerNotificationSystem.Broadcast($"{winner.Pawn.Name} won this round!", Channel.All);
        _judgePhase.OnNewJudge += (judge) => OnNewJudge?.Invoke(judge);
        _judgePhase.OnNewJudge += judge =>
        {
            PlayerNotificationSystem.ClearSubscriptions(Channel.Judge);
            PlayerNotificationSystem.ClearSubscriptions(Channel.Submitters);
            PlayerNotificationSystem.TrySubscribePlayer(Channel.Judge, judge);
            foreach (var submitters in _controllers.Where(c => c != judge))
            {
                PlayerNotificationSystem.TrySubscribePlayer(Channel.Submitters, submitters);
            }
        };
        checkWinnerPhase.OnWinnerFound += _ => context.Finished();
        checkWinnerPhase.OnWinnerFound += winner => Winner = winner;
        checkWinnerPhase.OnWinnerFound += winner => PlayerNotificationSystem.Broadcast($"{winner.Name} won the game", Channel.All);
        _judgePhase.Initialize();

        return context;
    }

    private static bool IsWinner(PlayerPawn player, int numPlayers)
    {
        switch (numPlayers)
        {
            case 4:
                if (player.Points >= 8) return true;
                break;
            case 5:
                if (player.Points >= 7) return true;
                break;
            case 6:
                if (player.Points >= 6) return true;
                break;
            case 7:
                if (player.Points >= 5) return true;
                break;
            case >= 8:
                if (player.Points >= 4) return true;
                break;
            default:
                throw new ArgumentException($"{numPlayers} player(s) are not enough to play this game variation.");

        }

        return false;
    }
}