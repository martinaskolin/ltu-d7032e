namespace ApplesToApples.Players;

public class PlayerManager
{
    public event Action<IPlayerController> OnJudgeChanged;

    public readonly List<IPlayerController> Players;
    public readonly List<PlayerPawn> Pawns;
    public readonly IPlayerController Judge;

}