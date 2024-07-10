using ApplesToApples.Cards;

namespace ApplesToApples.Players;

public class HumanController : IPlayerController
{
    private PlayerPawn _pawn;
    
    public Task<RedApple> Play(GreenApple greenApple)
    {
        throw new NotImplementedException();
    }

    public Task<RedApple> Judge(List<RedApple> redApples, GreenApple greenApple)
    {
        throw new NotImplementedException();
    }
}