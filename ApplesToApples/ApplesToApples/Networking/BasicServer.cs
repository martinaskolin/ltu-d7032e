namespace ApplesToApples.Networking;

public class BasicServer : IServer
{
    public event Action<IServerIO>? OnUserConnected;
    public event Action<IServerIO>? OnUserDisconnected;
    public async Task AcceptClients(int numClients)
    {
        throw new NotImplementedException();
    }

    // TODO: Continue
}