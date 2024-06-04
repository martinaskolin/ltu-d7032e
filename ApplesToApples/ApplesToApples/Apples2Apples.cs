using ApplesToApples.Cards;
using ApplesToApples.Players;

namespace ApplesToApples;

public class Apples2Apples
{
    private List<PlayerPawn> Players { get; set; } = new List<PlayerPawn>();
    private List<IPlayerController> Controllers = new List<IPlayerController>();
    private List<GreenApple> GreenApples { get; set; } = new List<GreenApple>();
    private List<RedApple> RedApples { get; set; } = new List<RedApple>();

    public List<PlayerPawn> GetPlayers()
    {
        return Players;
    }

    public List<IPlayerController> GetControllers()
    {
        return Controllers;
    }
}