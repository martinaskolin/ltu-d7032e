using System.Globalization;
using System.Reflection;
using System.Resources;
using ApplesToApples.Cards;

namespace ApplesToApples.Resources;

/// <summary>
/// A static class that provides access to the game's resources.
/// </summary>
public static class Resource
{
    private static ResourceManager _stringMng;
    
    static Resource()
    {
        _stringMng = new ResourceManager("ApplesToApples.Resources.Resources", Assembly.GetExecutingAssembly());
    }

    private static string? GetString(string name)
    {
        return _stringMng.GetString(name);
    }

    /// <summary>
    /// Reads the list of red apples from the resources file and returns them as a list of RedApple objects.
    /// </summary>
    public static List<RedApple> GetRedApples()
    {
        List<RedApple> redApples = new List<RedApple>();
        string[] nouns = GetString("RedApples").Split('\n') ?? throw new InvalidOperationException("Could not fetch Red Apples");
        
        foreach (string noun in nouns)
        {
            redApples.Add(new RedApple(noun));
        }

        return redApples;
    }
    
    /// <summary>
    /// Reads the list of green apples from the resources file and returns them as a list of GreenApple objects.
    /// </summary>
    public static List<GreenApple> GetGreenApples()
    {
        List<GreenApple> greenApples = new List<GreenApple>();
        string[] adjectives = GetString("GreenApples").Split('\n') ?? throw new InvalidOperationException("Could not fetch Green Apples");
        
        foreach (string adjective in adjectives)
        {
            greenApples.Add(new GreenApple(adjective));
        }

        return greenApples;
    }


}