using System.Net;
using System.Net.Sockets;
using ApplesToApples.Players.IO;

namespace ApplesToApples.Networking;

/// <summary>
/// Simple client class that connects to a server and sends and receives messages.
/// </summary>
public class Client
{

    /// <summary>
    /// Connects to a server at the given address and port.
    /// </summary>
    public void Connect(string address, int port)
    {
        IPAddress ipAddress = Dns.GetHostEntry(address).AddressList[0];
        IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
        Socket socket = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(ipEndPoint);

        SocketIO server = new SocketIO(socket);
        LocalIO io = new LocalIO();

        while (true)
        {
            string msg = server.Read() ?? "";
            
            if (msg.Contains(Server.SendData)) server.Write(io.Read() ?? "");
            else if (msg.Contains(Server.CloseConnection)) break;
            else io.Write(msg);
        }
        
        socket.Shutdown(SocketShutdown.Both);
    }
}