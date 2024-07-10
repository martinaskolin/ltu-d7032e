using ApplesToApples.Cards;
using ApplesToApples.Players;
using ApplesToApples.Utilities.ExtensionMethods;

namespace ApplesToApples.Game.Phases;

public class ReplenishPhase : IPhase
{
    private readonly List<IRedApple> _redApples;
    private readonly List<PlayerPawn> _pawns;
    
    public ReplenishPhase(List<IRedApple> redApples, List<PlayerPawn> pawns)
    {
        _redApples = redApples;
        _pawns = pawns;
    }
    
    /// <summary>
    /// Fills up the hand of each player to 7 cards
    /// </summary>
    public void Execute()
    {
        foreach (PlayerPawn pawn in _pawns)
        {
            while (pawn.Hand.Count < 7)
            {
                pawn.Hand.Add(_redApples.RemoveTop());
            }
        }
    }
}