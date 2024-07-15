using ApplesToApples.Cards;
using ApplesToApples.Networking;

namespace ApplesToApples.Players;

public class HumanController : IPlayerController
{
    public PlayerPawn Pawn { get; set; }
    
    private IServerIO _io;

    public HumanController(IServerIO io)
    {
        _io = io;
    }

    public Task<RedApple> Play(GreenApple greenApple)
    {
        throw new NotImplementedException();
    }

    public Task<RedApple> Judge(List<RedApple> redApples, GreenApple greenApple)
    {
        throw new NotImplementedException();
    }
}