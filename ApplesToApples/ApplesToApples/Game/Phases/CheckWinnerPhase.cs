using ApplesToApples.Players;

namespace ApplesToApples.Game.Phases;

public class CheckWinnerPhase : IPhase
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
                case >= 8:
                    if (player.GreenApples.Count >= 4) return player;
                    break;

            }
        }

        return null;
    }
}