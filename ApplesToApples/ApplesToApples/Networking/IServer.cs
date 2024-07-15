namespace ApplesToApples.Networking;

public interface IServer
{
    public event Action<IServerIO> OnUserConnected;
    public event Action<IServerIO> OnUserDisconnected;

    public Task AcceptClients(int numClients);
}