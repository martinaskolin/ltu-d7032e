using ApplesToApples.Game.Phases;

namespace ApplesToApples.Game.PhaseMachines;

public interface IPhaseMachine
{
    public bool MoveNext();
    public IGamePhase Current { get; }
}