using ApplesToApples.Cards;
using ApplesToApples.Players;
using ApplesToApples.Utilities;

namespace ApplesToApples.Game.Phases;

public class StartPhase : IPhase
{
    private Func<IPlayerController[]> FetchPlayers;
    private Func<IPlayerController> FetchJudge;
    
    public StartPhase(Func<IPlayerController[]> fetchPlayers, Func<IPlayerController> fetchJudge)
    {
        FetchPlayers = fetchPlayers;
        FetchJudge = fetchJudge;
    }
    
    public Task Execute()
    {
        IPlayerController judge = FetchJudge();
        List<IPlayerController> submitters = FetchPlayers().ToList();
        submitters.Remove(judge);

        foreach (var controller in submitters)
        {
            PlayerNotificationSystem.TryBroadcast(TextFormatter.Title("NEW ROUND"), controller);
        }
        
        PlayerNotificationSystem.TryBroadcast(TextFormatter.Title("NEW ROUND - JUDGE"), judge);
        PlayerNotificationSystem.TryBroadcast("Waiting for players...", judge);
        
        return Task.CompletedTask;
    }
}