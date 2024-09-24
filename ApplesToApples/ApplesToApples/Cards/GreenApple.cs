namespace ApplesToApples.Cards;

/// <summary>
/// Green Apple is one of the card types in Apples to Apples.
/// </summary>
public class GreenApple
{

    public readonly string Adjective;
    
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

    public override string ToString()
    {
        return Adjective;
    }
}