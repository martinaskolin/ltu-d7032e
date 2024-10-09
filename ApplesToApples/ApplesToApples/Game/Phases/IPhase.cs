namespace ApplesToApples.Game.Phases;

/// <summary>
/// Interface for game phases
/// </summary>
public interface IGamePhase
{
    /// <summary>
    /// Executes the phase
    /// </summary>
    public Task Execute();
}