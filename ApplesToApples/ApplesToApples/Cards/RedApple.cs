using ApplesToApples.Players;

namespace ApplesToApples.Cards;

public class RedApple : IRedApple
{

    public readonly string Noun;

    public RedApple(string noun)
    {
        Noun = noun;
        
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        RedApple other = (RedApple)obj;
        return Noun == other.Noun;
    }

    public override string ToString()
    {
        return Noun;
    }
}