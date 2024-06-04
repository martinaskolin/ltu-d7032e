using ApplesToApples.Cards;

namespace ApplesToApples.Players;

public interface IPlayerController
{
    public Task<IRedApple> SubmitRedApple(GreenApple greenApple);

    public Task<IRedApple> Judge(List<IRedApple> redApples, GreenApple greenApple);
}