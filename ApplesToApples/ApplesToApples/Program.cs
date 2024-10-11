using System.Net.Sockets;
using ApplesToApples.Game;
using ApplesToApples.Game.Variations;
using ApplesToApples.Networking;
using ApplesToApples.Players;
using ApplesToApples.Players.IO;

List<IPlayerController> controllers = new List<IPlayerController>();

LocalIO host = new LocalIO();
controllers.Add(new HumanController(host , new PlayerPawn()));
PlayerNotificationSystem.Subscribe(Channel.All, host.WriteLine);
PlayerNotificationSystem.Subscribe(Channel.Local, host.WriteLine);

// Host local lobby
if (args.Length == 0)
{
    while(controllers.Count < 4)
    {
        controllers.Add(new BotController(new PlayerPawn()));
    }
    
    StandardGame game = new StandardGame(controllers);

    
    bool done = false;
    while (!done)
    {
        done = !await game.Step();
    }
}

// Host online lobby
else if (int.TryParse(args[0], out int numOnlinePlayers))
{
    if (numOnlinePlayers < 1)
    {
        PlayerNotificationSystem.Broadcast("Invalid number of players, must be at least 1", Channel.Local);
        return;
    }
    
    Server server = new Server("localhost", 2048);

    server.OnUserConnected += io =>
    {
        controllers.Add(new HumanController(io, new PlayerPawn()));
        PlayerNotificationSystem.Subscribe(Channel.All, io.WriteLine);
        PlayerNotificationSystem.Subscribe(Channel.External, io.WriteLine);
    };
    server.OnUserConnected += _ => PlayerNotificationSystem.Broadcast("Player connected", Channel.All);
    
    PlayerNotificationSystem.Broadcast($"Waiting for {numOnlinePlayers} players to connect", Channel.All);
    await server.AcceptConnectionsAsync(numOnlinePlayers);
    
    while(controllers.Count < 4)
    {
        controllers.Add(new BotController(new PlayerPawn()));
    }
    
    StandardGame game = new StandardGame(controllers);

    try
    {
        bool done = false;
        while (!done)
        {
            done = !await game.Step();
        }
    }
    catch (SocketException)
    {
        PlayerNotificationSystem.Broadcast("Lost connection to one of the players, closing game", Channel.Local);
    }
    catch (Exception)
    {
        PlayerNotificationSystem.Broadcast("Game ended unexpectedly", Channel.Local);
    }
    
    PlayerNotificationSystem.Broadcast(Server.CloseConnection, Channel.External);
}

// Join as a client
else
{
    try
    {
        Client client = new Client();
        client.Connect(args[0], 2048);
    } catch (Exception e)
    {
        PlayerNotificationSystem.Broadcast("Unable to connect to server, make sure the server is running and the IP address is correct", Channel.Local);
    }
    
}


