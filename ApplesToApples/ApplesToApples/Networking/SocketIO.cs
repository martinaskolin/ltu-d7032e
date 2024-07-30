using System.Net.Sockets;
using System.Text;

namespace ApplesToApples.Networking;

public class SocketIO : ClientIO
{
    private readonly Socket _socket;

    public SocketIO(Socket socket)
    {
        _socket = socket;
    }
    
    public override void Write(string message)
    {
        _socket.Send(Encoding.UTF8.GetBytes(message));
    }

    public override string? Read()
    {
        var buffer = new byte[1_024];
        var received = _socket.Receive(buffer, SocketFlags.None);
        return Encoding.UTF8.GetString(buffer, 0, received);
    }

    public override async Task<string?> ReadAsync()
    {
        var buffer = new byte[1_024];
        var received = await _socket.ReceiveAsync(buffer, SocketFlags.None);
        return Encoding.UTF8.GetString(buffer, 0, received);
    }
    
}