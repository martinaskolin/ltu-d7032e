using ApplesToApples.Cards;

namespace ApplesToApples.Players;

public interface IPlayerController
{
    public Task<RedApple> Play(GreenApple greenApple);
    public Task<RedApple> Judge(List<RedApple> redApples, GreenApple greenApple);
}