namespace ApplesToApples.Networking;

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

    public override Task<string?> ReadAsync()
    {
        // It's a bit wasteful dedicating a new thread for this but it seems like the best solution for now
        // As the project grows you would probably like to rewrite this part
        return Task.Run(() => Console.ReadLine());
    }
}