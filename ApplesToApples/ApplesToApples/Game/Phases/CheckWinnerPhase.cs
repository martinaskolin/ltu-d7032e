using ApplesToApples.Players;

namespace ApplesToApples.Game.Phases;

/// <summary>
/// Phase for checking if any player has won the game
/// </summary>
public class CheckWinnerPhase : IGamePhase
{
    /// <summary>
    /// Invoked when a winner is found
    /// </summary>
    public event Action<PlayerPawn> OnWinnerFound;

    private readonly List<PlayerPawn> _players;
    private Func<PlayerPawn, int, bool> IsWinner;
    
    public CheckWinnerPhase(List<PlayerPawn> pawns, Func<PlayerPawn, int, bool> isWinner)
    {
        _players = pawns;
        IsWinner = isWinner;
    }

    public async Task Execute()
    {
        foreach (PlayerPawn player in _players)
        {
            if (IsWinner(player, _players.Count))
            {
                OnWinnerFound?.Invoke(player);
                break;
            }
        }
    }

    
}