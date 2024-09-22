using ApplesToApples.Cards;

namespace ApplesToApples.Players;

public interface IPlayerController
{
    public PlayerPawn Pawn { get; }
    public Task<RedApple> Play(GreenApple greenApple);
    public Task<(IPlayerController, RedApple)> Judge(List<(IPlayerController, RedApple)> submissions, GreenApple greenApple);
}