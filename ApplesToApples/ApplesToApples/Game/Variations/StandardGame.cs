using ApplesToApples.Cards;
using ApplesToApples.Game.Phases;
using ApplesToApples.Players;
using ApplesToApples.Resources;
using ApplesToApples.Utilities.ExtensionMethods;

namespace ApplesToApples.Game.Variations;

public class StandardGame
{
    public readonly List<IRedApple> RedApples;
    public readonly List<GreenApple> GreenApples;

    private readonly IPhaseIterator _iterator;
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

        _iterator = CreatePhases(); // TODO: Redo Standard Game set up, look at creation patterns...


    }

    public void Step()
    {
        if (_iterator.MoveNext()) _iterator.Current.Execute(); // TODO: Endless loop (we dont check for winner)
    }

    public IPhaseIterator CreatePhases()
    {
        // Init phases
        ReplenishPhase replenishPhase = new ReplenishPhase(RedApples, _playerManager.Pawns);
        DrawPhase drawPhase = new DrawPhase(GreenApples);
        SubmitPhase submitPhase = new SubmitPhase(_playerManager.Players);
        JudgePhase judgePhase = new JudgePhase(_playerManager.Players);

        // Set dynamic dependencies with events (observer pattern with dependency injection)
        drawPhase.OnDraw += submitPhase.SetGreenApple;
        drawPhase.OnDraw += judgePhase.SetGreenApple;
        submitPhase.OnCardsSubmitted += judgePhase.SetSubmittedCards;
        judgePhase.OnWinnerSelected += redApple => redApple.Owner.GreenApples.Add(drawPhase.Current);
        
        return new PhaseListIterator(new List<IPhase>(){
            replenishPhase,
            drawPhase,
            submitPhase,
            judgePhase
        });
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