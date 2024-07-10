using System.Globalization;
using System.Reflection;
using System.Resources;
using ApplesToApples.Cards;

namespace ApplesToApples.Resources;

public static class Resource
{
    private static ResourceManager _stringMng;
    
    static Resource()
    {
        _stringMng = new ResourceManager("ApplesToApples.Resources.Resources", Assembly.GetExecutingAssembly());
        
        // TODO: Change this mess
        // Set language
        try
        {
            GetString("Menu.Title");
        }
        catch (MissingManifestResourceException)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en");
        }
    }

    public static string? GetString(string name)
    {
        return _stringMng.GetString(name);
    }

    public static List<IRedApple> GetRedApples()
    {
        List<IRedApple> redApples = new List<IRedApple>();
        string[] nouns = GetString("RedApples").Split('\n') ?? throw new InvalidOperationException("Could not fetch Red Apples");
        
        foreach (string noun in nouns)
        {
            redApples.Add(new RedApple(noun));
        }

        return redApples;
    }
    
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