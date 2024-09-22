using ApplesToApples.Cards;
using ApplesToApples.Players;

namespace ApplesToApples.Game.Phases;

public class JudgePhase : IGamePhase
{
    
    public IPlayerController CurrentJudge => _controllers[_index];

    // Judge related
    private readonly List<IPlayerController> _controllers;
    private int _index;
    
    // Verdict related
    private List<(IPlayerController, RedApple)>? _submissions;
    private GreenApple? _greenApple;
    

    public JudgePhase(List<IPlayerController> controllers)
    {
        _index = new Random().Next(0, controllers.Count);
        _controllers = controllers;
    }
    
    public async Task Execute()
    {
        IPlayerController judge = _controllers[_index];
        List<(IPlayerController, RedApple)> submissions = _submissions ?? throw new NullReferenceException("No submissions found");
        GreenApple greenApple = _greenApple ?? throw new NullReferenceException("No green apple found");

        (var winner, var submission) = await judge.Judge(submissions, greenApple); // TODO: Continue
        winner.Pawn.GivePoint(greenApple);
        
        //OnVerdict?.Invoke(await judge.Judge(submittedCards, greenApple));
        _index = (_index + 1) % (_controllers.Count - 1);
    }

    public void SetSubmissions(List<(IPlayerController,RedApple)> submissions)
    {
        _submissions = submissions;
    }

    public void SetGreenApple(GreenApple greenApple)
    {
        _greenApple = greenApple;
    }
}