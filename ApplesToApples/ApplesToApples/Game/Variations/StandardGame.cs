using ApplesToApples.Cards;
using ApplesToApples.Game.Phases;
using ApplesToApples.Players;
using ApplesToApples.Resources;
using ApplesToApples.Utilities.ExtensionMethods;

namespace ApplesToApples.Game.Variations;

public class StandardGame
{
    private PlayerManager _playerManager;
    public List<IPhase> Phases { get; private set; }
    private readonly List<IRedApple> _redApples = new List<IRedApple>();
    private readonly List<GreenApple> _greenApples = new List<GreenApple>();
    private List<PlayerPawn> _pawns = new List<PlayerPawn>();
    
    public StandardGame(int numPlayers)
    {
        _playerManager = new PlayerManager();
        
        // Read all the green apples (adjectives) from a file and add to the green apples deck
        _greenApples = Resource.GetGreenApples();
        
        // Read all the red apples (nouns) from a file and add to the red apples deck
        _redApples = Resource.GetRedApples();
        
        // Shuffle both the green apples and red apples decks
        _greenApples.Shuffle();
        _redApples.Shuffle();

        // Init phases
        ReplenishPhase replenishPhase = new ReplenishPhase(_redApples, _playerManager.Pawns);
        DrawPhase drawPhase = new DrawPhase(_greenApples);
        SubmitPhase submitPhase = new SubmitPhase(_playerManager.Players);
        JudgePhase judgePhase = new JudgePhase();

        // Set dynamic dependencies with events (observer pattern)
        drawPhase.OnDraw += submitPhase.SetGreenApple;
        submitPhase.OnCardsSubmitted += judgePhase.SetSubmittedCards;
        _playerManager.OnJudgeChanged += judgePhase.SetJudge;
        judgePhase.OnWinnerSelected += redApple =>
        {
            redApple.Owner.GreenApples.Add(drawPhase.Current);
        };
        
        
        Phases = new List<IPhase>
        {
            replenishPhase,
            drawPhase,
            submitPhase,
            judgePhase
        };
    }

    public static PlayerPawn? CheckWinner(List<PlayerPawn> players)
    {
        if (players.Count < 4) throw new ArgumentException("Number of players should be at least 4");

        foreach (PlayerPawn player in players)
        {
            switch (players.Count)
            {
                case 4:
                    if (player.GreenApples.Count >= 8) return player;
                    break;
                case 5:
                    if (player.GreenApples.Count >= 7) return player;
                    break;                
                case 6:
                    if (player.GreenApples.Count >= 6) return player;
                    break;
                case 7:
                    if (player.GreenApples.Count >= 5) return player;
                    break;
                default:
                    if (player.GreenApples.Count >= 4) return player;
                    break;
                
            }
        }

        return null;
    }
}