using ApplesToApples.Cards;
using ApplesToApples.Players;

namespace ApplesToApples.Game.Phases;

public class SubmitPhase : IGamePhase
{
    // Events
    public event Action<List<(IPlayerController, RedApple)>> OnSubmissons;

    // Dynamic Variables
    private Func<IPlayerController[]> FetchSubmitters;
    private Func<GreenApple> FetchGreenApple;

    public SubmitPhase(Func<IPlayerController[]> fetchSubmitters, Func<GreenApple> fetchGreenApple)
    {
        FetchSubmitters = fetchSubmitters;
        FetchGreenApple = fetchGreenApple;
    }
    
    public async Task Execute()
    {
        GreenApple greenApple = FetchGreenApple();
        List<(IPlayerController, Task<RedApple>)> tasks = new List<(IPlayerController, Task<RedApple>)>();
        
        foreach (IPlayerController controller in FetchSubmitters())
        {
            tasks.Add((controller, controller.Play(greenApple)));
        }

        await Task.WhenAll(tasks.Select(tuple => tuple.Item2).ToArray());
        
        OnSubmissons?.Invoke(tasks.Select(tuple => (tuple.Item1, tuple.Item2.Result)).ToList());
    }
    
}