using ApplesToApples.Game.Phases;

namespace ApplesToApples.Game;

public interface IPhaseIterator
{
    public bool MoveNext();
    public IPhase Current { get;}
}