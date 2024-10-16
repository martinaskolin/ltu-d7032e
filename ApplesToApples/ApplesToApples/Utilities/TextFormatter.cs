namespace ApplesToApples.Utilities;

public static class TextFormatter
{
    /// <summary>
    /// Formats a message as a title with a boarder of "*" characters.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string Title(string message)
    {
        string result = "";
        
        // Get the console window width
        int windowWidth = Console.WindowWidth;
        int messageWidth = message.Length + 4; // Adding space for "* " before and " *" after the message
        
        int leftPadding = Math.Max((windowWidth - messageWidth) / 2, 0);
        
        result += new string('*', windowWidth) + "\n"; // Top boarder
        result += new string(' ', leftPadding) + message + "\n"; // Title
        result += new string('*', windowWidth) + "\n"; // Bottom boarder

        return result;
    }
}