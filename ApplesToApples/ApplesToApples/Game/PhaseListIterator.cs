using ApplesToApples.Game.Phases;

namespace ApplesToApples.Game;

public class PhaseListIterator : IPhaseIterator
{
    private List<IPhase> _phases;
    private Func<bool> _checkFinished;
    private int _index = 0;

    public PhaseListIterator(List<IPhase> phases, Func<bool> checkFinished)
    {
        _phases = phases;
        _checkFinished = checkFinished;
    }
    
    public bool MoveNext()
    {
        _index = (_index + 1) % _phases.Count;
        return _checkFinished.Invoke();
    }

    public IPhase Current => _phases[_index-1];
}