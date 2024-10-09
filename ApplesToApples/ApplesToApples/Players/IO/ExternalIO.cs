using System.Net.Sockets;
using ApplesToApples.Networking;

namespace ApplesToApples.Players.IO;

/// <summary>
/// Is an IO that reads from the server and writes to the server.
/// Use this instead of SocketIO when on the server side.
/// </summary>
public class ExternalIO : SocketIO
{
    public ExternalIO(Socket socket) : base(socket) { }

    public override string? Read()
    {
        Write(Server.SendData);
        return base.Read();
    }

    protected override Task<string?> ReadAsync()
    {
        Write(Server.SendData);
        return base.ReadAsync();
    }
}