using ApplesToApples.Game.Phases;

namespace ApplesToApples.Game.PhaseMachines;

public interface IPhaseMachine
{
    /// <summary>
    /// Moves to the next phase
    /// </summary>
    /// <returns>if there is a next phase or not</returns>
    public bool MoveNext();
    
    /// <summary>
    /// Current phase, usually called after calling MoveNext
    /// </summary>
    public IGamePhase Current { get; }
}