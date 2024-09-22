using ApplesToApples.Players;

namespace ApplesToApples.Game.Phases;

public class CheckWinnerPhase : IGamePhase
{
    public event Action<PlayerPawn> OnWinnerFound;

    private readonly List<PlayerPawn> _players;

    public CheckWinnerPhase(List<PlayerPawn> pawns)
    {
        _players = pawns;
    }

    public async Task Execute()
    {
        PlayerPawn? winner = CheckWinner();
        if (winner != null) OnWinnerFound?.Invoke(winner);
    }

    private PlayerPawn? CheckWinner()
    {
        if (_players.Count < 4) throw new ArgumentException("Number of players should be at least 4");

        foreach (PlayerPawn player in _players)
        {
            switch (_players.Count)
            {
                case 4:
                    if (player.Points >= 8) return player;
                    break;
                case 5:
                    if (player.Points >= 7) return player;
                    break;
                case 6:
                    if (player.Points >= 6) return player;
                    break;
                case 7:
                    if (player.Points >= 5) return player;
                    break;
                case >= 8:
                    if (player.Points >= 4) return player;
                    break;

            }
        }

        return null;
    }
}