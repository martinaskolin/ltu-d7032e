namespace ApplesToApples.Game.Phases;

/// <summary>
/// Interface for game phases
/// </summary>
public interface IPhase
{
    /// <summary>
    /// Executes the phase
    /// </summary>
    public Task Execute();
}