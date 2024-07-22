using ApplesToApples.Cards;
using ApplesToApples.Players;

namespace ApplesToApples.Game.Phases;

public class JudgePhase : IPhase
{
    public IPlayerController CurrentJudge => _controllers[_index];

    private readonly List<IPlayerController> _controllers;
    private int _index;
    private List<RedApple>? _submittedCards;
    private GreenApple? _greenApple;
    public event Action<RedApple>? OnWinnerSelected;

    public JudgePhase(List<IPlayerController> controllers)
    {
        _index = new Random().Next(0, controllers.Count);
        _controllers = controllers;
    }
    
    public async void Execute()
    {
        IPlayerController judge = _controllers[_index];
        List<RedApple> submittedCards = _submittedCards ?? throw new NullReferenceException("Submitted cards has not been set");
        GreenApple greenApple = _greenApple ?? throw new NullReferenceException("Green apple has not been set");
        
        OnWinnerSelected?.Invoke(await judge.Judge(submittedCards, greenApple));
        _index++;
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