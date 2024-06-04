using ApplesToApples.Cards;
using ApplesToApples.Players;

namespace ApplesToApples.States;

public class SubmitState : IGameState
{

    private readonly Func<List<IPlayerController>> _getControllers;
    private readonly Func<GreenApple> _getGreenApple;
    private List<IRedApple> _redApples = new List<IRedApple>();

    public SubmitState(Func<List<IPlayerController>> getControllers, Func<GreenApple> getGreenApple)
    {
        _getControllers = getControllers;
        _getGreenApple = getGreenApple;
    }
    
    public async void Execute()
    {
        _redApples = new List<IRedApple>();
        GreenApple greenApple = _getGreenApple.Invoke();
        List<IPlayerController> players = _getControllers.Invoke();

        var tasks = new Task<IRedApple>[players.Count];
        foreach (var item in players.Select((value, i) => new { i, value }))
        {
            tasks[item.i] = item.value.SubmitRedApple(greenApple);
        }

        await Task.WhenAll(tasks);

        foreach (Task<IRedApple> task in tasks)
        {
            _redApples.Add(task.Result);
        }
    }

    public List<IRedApple> GetRedApples()
    {
        return _redApples;
    }
}