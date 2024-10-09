using ApplesToApples.Players;

namespace ApplesToApples.Cards;

public class RedApple : IRedApple
{
    private static int _numCards = 0;
    private readonly string _noun;
    private readonly int _id;

    public RedApple(string noun)
    {
        _noun = noun;
        _id = _numCards++;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        RedApple other = (RedApple)obj;
        return _id == other._id;
    }

    /// <returns>Noun of the red apple</returns>
    public override string ToString()
    {
        return _noun;
    }
}