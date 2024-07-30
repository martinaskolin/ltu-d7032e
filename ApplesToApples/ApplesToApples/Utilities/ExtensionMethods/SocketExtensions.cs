using System.Net;
using System.Net.Sockets;

namespace ApplesToApples.Utilities.ExtensionMethods;

public static class SocketExtensions
{
    /// <summary>
    /// Connect to provided address and port.
    /// </summary>
    /// <param name="socket"></param>
    /// <param name="address">address to connect to</param>
    /// <param name="port">port to connect to</param>
    public static void Connect(this Socket socket, string address, int port)
    {
        IPAddress ipAddress = Dns.GetHostEntry(address).AddressList[0];
        IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
        socket.Connect(ipEndPoint);
    }
    
    /// <summary>
    /// Pings Socket to check if it's still connected or not.
    /// </summary>
    /// <param name="socket"></param>
    /// <returns>True = Connected, False = Disconnected</returns>
    public static bool IsConnected(this Socket socket)
    {
        try
        {
            return !(socket.Available == 0 && socket.Poll(1, SelectMode.SelectRead));
        }
        catch (SocketException) { return false; }
    }
}