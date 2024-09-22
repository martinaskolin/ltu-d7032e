using ApplesToApples.Cards;
using ApplesToApples.Players;
using ApplesToApples.Utilities.ExtensionMethods;

namespace ApplesToApples.Game.Phases;

public class ReplenishPhase : IGamePhase
{
    private readonly List<IRedApple> _redApples;
    private readonly List<PlayerPawn> _players;
    
    public ReplenishPhase(List<IRedApple> redApples, List<PlayerPawn> pawns)
    {
        _redApples = redApples;
        _players = pawns;
    }
    
    /// <summary>
    /// Fills up the hand of each player to 7 cards
    /// </summary>
    public async Task Execute()
    {
        foreach (PlayerPawn player in _players)
        {
            while (player.NumberOfCardsInHand < 7)
            {
                player.GiveCard(_redApples.RemoveTop());
            }
        }
    }
}