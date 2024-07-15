using ApplesToApples.Game.Phases;

namespace ApplesToApples.Game;

public class PhaseListIterator : IPhaseIterator
{
    private bool _finished;
    private readonly List<IPhase> _phases;
    private int _index = 0;

    public PhaseListIterator(List<IPhase> phases)
    {
        _phases = phases;
    }
    
    public bool MoveNext()
    {
        _index = (_index + 1) % _phases.Count;
        return !_finished;
    }

    public void Finished()
    {
        _finished = true;
    }

    public IPhase Current => _phases[_index-1];
}