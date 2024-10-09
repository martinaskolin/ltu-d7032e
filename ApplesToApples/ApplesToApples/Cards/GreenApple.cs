namespace ApplesToApples.Cards;

/// <summary>
/// Green Apple is one of the card types in Apples to Apples.
/// </summary>
public class GreenApple
{

    /// <summary>
    /// The content (Adjective) usually associated with the green apple in the game
    /// </summary>
    public readonly string Adjective;
    
    /// <param name="adjective">The content (Adjective) usually associated with the green apple in the game</param>
    public GreenApple(string adjective)
    {
        Adjective = adjective;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        GreenApple other = (GreenApple)obj;
        return Adjective == other.Adjective;
    }
    
    /// <returns>Adjective of the green apple</returns>
    public override string ToString()
    {
        return Adjective;
    }
}