using ApplesToApples.Cards;

namespace ApplesToApples.Game;

public class ApplesToApples
{
    public IPhaseIterator Iterator;
    
    public List<GreenApple> GreenApples = new List<GreenApple>();
    public List<IRedApple> RedApples = new List<IRedApple>();
}