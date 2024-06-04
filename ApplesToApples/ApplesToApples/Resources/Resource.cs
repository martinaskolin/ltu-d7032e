using System.Globalization;
using System.Reflection;
using System.Resources;

namespace ApplesToApples.Resources;

public static class Resource
{
    private static ResourceManager _stringMng;
    
    static Resource()
    {
        _stringMng = new ResourceManager("ApplesToApples.Application.Resources.strings", Assembly.GetExecutingAssembly());
        
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

    public static string GetRedApples()
    {
        return GetString("RedApples") ?? throw new InvalidOperationException("Could not fetch Red Apples");
    }
    
    public static string GetGreenApples()
    {
        return GetString("GreenApples") ?? throw new InvalidOperationException("Could not fetch Green Apples");
    }


}