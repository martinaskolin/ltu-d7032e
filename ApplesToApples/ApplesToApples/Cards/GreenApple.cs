namespace ApplesToApples.Cards;

public class GreenApple
{
    private readonly string _adjective;
    
    public GreenApple(string adjective)
    {
        _adjective = adjective;
    }

    public override string ToString()
    {
        return _adjective;
    }
}