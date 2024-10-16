namespace ApplesToApples.Players.IO;

/// <summary>
/// Abstract class for handling input and output (IO) for a client.
/// </summary>
public abstract class ClientIO
{
    /// <summary>
    /// Write a message to the IO.
    /// </summary>
    /// <param name="message">Message to write</param>
    public abstract void Write(string message);
    
    /// <summary>
    /// Read a message from the IO (Synchronous).
    /// </summary>
    /// <returns>Message read from the IO</returns>
    public abstract string? Read();
    
    /// <summary>
    /// Read a message from the IO (Asynchronous).
    /// </summary>
    /// <returns>Message read from the IO</returns>
    protected abstract Task<string?> ReadAsync();

    private delegate bool TryParseDelegate<TIn, TOut>(TIn input, out TOut result);

    private async Task<T> ReadAsync<T>(TryParseDelegate<string?, T> parser, string prefix)
    {
        bool valid;
        T value;

        do
        {
            Write(prefix);
            var input = await ReadAsync();
            valid = parser(input, out value);
        } while (!valid);

        return value;
    }

    /// <summary>
    /// Gets a valid integer from the IO.
    /// </summary>
    /// <param name="min">min value of the integer (inclusive)</param>
    /// <param name="max">max value of the integer (inclusive)</param>
    /// <param name="prefix">message before input, gives context to the user</param>
    /// <returns></returns>
    public Task<int> ReadAsync(int min, int max, string prefix="")
    {
        return ReadAsync((string? s, out int res) =>
        {
            if (!int.TryParse(s, out res)) WriteLine("Answer should be a number!");
            if (min > res || res > max) WriteLine($"Answer should be within the range of {min} and {max}");
            return int.TryParse(s, out res) && min <= res && res <= max;
        }, prefix);
    }
    
    /// <summary>
    /// Write a message to the IO with a line break at the end.
    /// </summary>
    /// <param name="message">Message to write</param>
    public void WriteLine(string message) {Write($"{message}\n");}
    
}