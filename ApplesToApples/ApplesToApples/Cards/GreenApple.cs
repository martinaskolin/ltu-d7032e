namespace ApplesToApples.Cards;

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
}