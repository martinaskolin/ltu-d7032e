using System.Net;
using System.Net.Sockets;

namespace ApplesToApples.Networking;

public class Server
{
    public event Action<ClientIO>? OnUserConnected;

    public static readonly string SendData = "SEND_DATA";
    public static readonly string CloseConnection = "BYE";
    
    private readonly TcpListener _listener;
    
    /// <summary>
    /// Creates a server given an address or DNS look-up name as well as a port number.
    /// </summary>
    /// <param name="address">Address or DNS look-up name of server</param>
    /// <param name="port">Port number of server</param>
    public Server(string address, int port)
    {
        IPAddress ipAddress = Dns.GetHostEntry(address).AddressList[0];
        IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
        _listener = new TcpListener(ipEndPoint);
    }

    /// <summary>
    /// Accepts a given number of connections to the server asynchronously.
    /// </summary>
    /// <param name="numConnections">Number of connections to accept</param>
    public async Task AcceptConnectionsAsync(int numConnections)
    {
        _listener.Start();
        for (int i = 0; i < numConnections; i++)
        {
            Socket newConnection = await _listener.AcceptSocketAsync();
            OnUserConnected?.Invoke(new SocketIO(newConnection));
        }
        _listener.Stop();
    }
    
}