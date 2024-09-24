using System.Net.Sockets;
using System.Text;

namespace ApplesToApples.Players.IO;

public class SocketIO : ClientIO
{
    private readonly Socket _socket;

    public SocketIO(Socket socket)
    {
        _socket = socket;
    }
    
    public override void Write(string message)
    {
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        byte[] lengthBytes = BitConverter.GetBytes(messageBytes.Length);

        // Sends length of message followed by message
        _socket.Send(lengthBytes);
        _socket.Send(messageBytes);
    }

    public override string? Read()
    {
        // Receive length of message
        byte[] lengthBuffer = new byte[4];
        _socket.Receive(lengthBuffer, lengthBuffer.Length, SocketFlags.None);
        int messageLength = BitConverter.ToInt32(lengthBuffer);
        
        // Read message
        byte[] messageBuffer = new byte[messageLength];
        var received = _socket.Receive(messageBuffer, messageBuffer.Length, SocketFlags.None);
        return Encoding.UTF8.GetString(messageBuffer, 0, received);
    }

    public override async Task<string?> ReadAsync()
    {
        // Receive length of message
        byte[] lengthBuffer = new byte[4];
        await _socket.ReceiveAsync(lengthBuffer, SocketFlags.None);
        int messageLength = BitConverter.ToInt32(lengthBuffer);
        
        // Read message
        byte[] messageBuffer = new byte[messageLength];
        var received = await _socket.ReceiveAsync(messageBuffer, SocketFlags.None);
        return Encoding.UTF8.GetString(messageBuffer, 0, received);
    }
    
}