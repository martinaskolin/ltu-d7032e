using ApplesToApples.Game.Phases;

namespace ApplesToApples.Game.PhaseMachines;

/// <summary>
/// Loops over a list of phases sequentially until the method Finished is externally called.
/// </summary>
public class SequentialMachine : IPhaseMachine
{
    private int _index = -1;
    private readonly List<IPhase> _phases;
    private bool _finished = false;

    public SequentialMachine(List<IPhase> phases)
    {
        _phases = phases;
    }
    
    public bool MoveNext()
    {
        _index = (_index + 1) % _phases.Count;
        return !_finished;
    }

    /// <summary>
    /// Call this when the game is over
    /// </summary>
    public void Finished()
    {
        _finished = true;
    }

    public IPhase Current => _phases[_index];
}