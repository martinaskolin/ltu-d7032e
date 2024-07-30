namespace ApplesToApples.Networking;

public abstract class ClientIO
{
    /// <summary>
    /// Write a message to the IO.
    /// </summary>
    /// <param name="message">Message to write</param>
    public abstract void Write(string message);

    /// <summary>
    /// Write a message to the IO.
    /// </summary>
    /// <param name="message">Message to write</param>
    /// <param name="arg0">Argument to fill in "{0}" occurrence in string</param>
    public void Write(string message, object? arg0)
    {
        Write(String.Format(message, arg0));
    }
    
    /// <summary>
    /// Write a message to the IO.
    /// </summary>
    /// <param name="message">Message to write</param>
    /// <param name="arg0">Argument to fill in "{0}" occurrence in string</param>
    /// <param name="arg1">Argument to fill in "{1}" occurrence in string</param>
    public void Write(string message, object? arg0, object? arg1)
    {
        Write(String.Format(message, arg0, arg1));
    }
    
    /// <summary>
    /// Write a message to the IO.
    /// </summary>
    /// <param name="message">Message to write</param>
    /// <param name="arg">Arguments to fill in "{x}" occurrences in string</param>
    public void Write(string message, params object?[] arg)
    {
        Write(String.Format(message, arg));
    }
    
    /// <summary>
    /// Read a message from the IO (Synchronous).
    /// </summary>
    /// <returns>Message read from the IO</returns>
    public abstract string? Read();
    
    /// <summary>
    /// Read a message from the IO (Asynchronous).
    /// </summary>
    /// <returns>Message read from the IO</returns>
    public abstract Task<string?> ReadAsync();
    
    /// <summary>
    /// Write a message to the IO with a line break at the end.
    /// </summary>
    /// <param name="message">Message to write</param>
    public void WriteLine(string message) {Write($"{message}\n");}
    
    /// <summary>
    /// Write a message to the IO.
    /// </summary>
    /// <param name="message">Message to write</param>
    /// <param name="arg0">Argument to fill in "{0}" occurrence in string</param>
    public void WriteLine(string message, object? arg0)
    {
        WriteLine(String.Format(message, arg0));
    }
    
    /// <summary>
    /// Write a message to the IO.
    /// </summary>
    /// <param name="message">Message to write</param>
    /// <param name="arg0">Argument to fill in "{0}" occurrence in string</param>
    /// <param name="arg1">Argument to fill in "{1}" occurrence in string</param>
    public void WriteLine(string message, object? arg0, object? arg1)
    {
        WriteLine(String.Format(message, arg0, arg1));
    }
    
    /// <summary>
    /// Write a message to the IO.
    /// </summary>
    /// <param name="message">Message to write</param>
    /// <param name="arg">Arguments to fill in "{x}" occurrences in string</param>
    public void WriteLine(string message, params object?[] arg)
    {
        WriteLine(String.Format(message, arg));
    }
    
}