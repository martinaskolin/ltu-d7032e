using System.Net.Sockets;
using ApplesToApples.Networking;

namespace ApplesToApples.Players.IO;

public class ExternalIO : SocketIO
{
    public ExternalIO(Socket socket) : base(socket) { }

    public override string? Read()
    {
        Write(Server.SendData);
        return base.Read();
    }

    public override Task<string?> ReadAsync()
    {
        Write(Server.SendData);
        return base.ReadAsync();
    }
}