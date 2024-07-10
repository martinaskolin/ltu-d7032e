using ApplesToApples.Cards;
using ApplesToApples.Players;

namespace ApplesToApples.Game.Phases;

public class JudgePhase : IPhase
{
    private IPlayerController? _judge;
    private List<RedApple>? _submittedCards;
    private GreenApple? _greenApple;
    public event Action<RedApple>? OnWinnerSelected;
    
    public async void Execute()
    {
        IPlayerController judge = _judge ?? throw new NullReferenceException("Judge has not been set");
        List<RedApple> submittedCards = _submittedCards ?? throw new NullReferenceException("Submitted cards has not been set");
        GreenApple greenApple = _greenApple ?? throw new NullReferenceException("Green apple has not been set");
        
        OnWinnerSelected?.Invoke(await judge.Judge(submittedCards, greenApple));
    }

    public void SetJudge(IPlayerController judge)
    {
        _judge = judge;
    }

    public void SetSubmittedCards(List<RedApple> submittedCards)
    {
        _submittedCards = submittedCards;
    }

    public void SetGreenApple(GreenApple greenApple)
    {
        _greenApple = greenApple;
    }
}