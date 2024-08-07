using ApplesToApples.Cards;
using ApplesToApples.Players;

namespace ApplesToApples.Game.Phases;

public class SubmitPhase : IPhase
{
    public event Action<List<RedApple>>? OnCardsSubmitted;

    private GreenApple? _greenApple;
    private readonly List<IPlayerController> _controllers;

    public SubmitPhase(List<IPlayerController> controllers)
    {
        _controllers = controllers;
    }
    
    public async Task Execute()
    {
        List<RedApple> results = new List<RedApple>();
        List<Task<RedApple>> tasks = new List<Task<RedApple>>();
        foreach (IPlayerController controller in _controllers)
        {
            tasks.Add(controller.Play(_greenApple ?? throw new NullReferenceException("No green apple has been set")));
        }

        await Task.WhenAll(tasks.ToArray());

        foreach (Task<RedApple> task in tasks)
        {
            results.Add(task.Result);
        }
        
        OnCardsSubmitted?.Invoke(results);
    }

    public void SetGreenApple(GreenApple greenApple)
    {
        _greenApple = greenApple;
    }
    
}