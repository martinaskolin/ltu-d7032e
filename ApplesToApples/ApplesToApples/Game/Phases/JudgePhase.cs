using ApplesToApples.Cards;
using ApplesToApples.Players;

namespace ApplesToApples.Game.Phases;

/// <summary>
/// Phase responsible for selecting a judge and judging submissions.
/// </summary>
public class JudgePhase : IPhase
{
    /// <summary>
    /// Invoked when a new judge is selected. Contains the new judge.
    /// </summary>
    public event Action<IPlayerController> OnNewJudge;
    
    /// <summary>
    /// Invoked when the judge has made a decision. Contains the winner, the winning submission, and the green apple.
    /// </summary>
    public event Action<IPlayerController, RedApple, GreenApple> OnVerdict; 
    
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
        OnNewJudge?.Invoke(judge);
        List<(IPlayerController, RedApple)> submissions = _submissions ?? throw new NullReferenceException("No submissions found");
        GreenApple greenApple = _greenApple ?? throw new NullReferenceException("No green apple found");

        (var winner, var submission) = await judge.Judge(submissions, greenApple);
        
        OnVerdict?.Invoke(winner, submission, greenApple);
        _index = (_index + 1) % _controllers.Count;
    }

    /// <summary>
    /// Should be called before the phase is executed
    /// </summary>
    /// <param name="submissions">Submissions to judge</param>
    public void SetSubmissions(List<(IPlayerController,RedApple)> submissions)
    {
        _submissions = submissions;
    }

    /// <summary>
    /// Should be called before the phase is executed
    /// </summary>
    /// <param name="greenApple">Green apple to judge submissions by</param>
    public void SetGreenApple(GreenApple greenApple)
    {
        _greenApple = greenApple;
    }
}