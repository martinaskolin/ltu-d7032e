namespace ApplesToApples.Players;

public class PlayerManager
{
    public readonly List<IPlayerController> Players = new List<IPlayerController>();
    public readonly List<PlayerPawn> Pawns = new List<PlayerPawn>();

    public void AddPlayer(IPlayerController controller)
    {
        controller.Pawn = new PlayerPawn();
        Players.Add(controller);
        Pawns.Add(controller.Pawn);
    }
}