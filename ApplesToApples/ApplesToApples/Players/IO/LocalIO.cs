namespace ApplesToApples.Players.IO;

/// <summary>
/// Responsible for handling input and output from the console on the local machine
/// </summary>
public class LocalIO : ClientIO
{
    public override void Write(string message)
    {
        Console.Write(message);
    }

    public override string? Read()
    {
        return Console.ReadLine();
    }

    protected override Task<string?> ReadAsync()
    {
        // It's a bit wasteful dedicating a new thread for this but it seems like the best solution for now
        // As the project grows you would probably like to rewrite this part
        return Task.Run(() => Console.ReadLine());
    }
}