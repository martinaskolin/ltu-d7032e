using ApplesToApples.Game;
using ApplesToApples.Game.Variations;
using ApplesToApples.Networking;
using ApplesToApples.Players;
using ApplesToApples.Players.IO;

List<IPlayerController> controllers = new List<IPlayerController>();

LocalIO host = new LocalIO();
controllers.Add(new HumanController(host, new PlayerPawn()));
PlayerNotificationSystem.Subscribe(Channel.All, host.WriteLine);

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

    bool done = false;
    while (!done)
    {
        done = !await game.Step();
    }
    
    PlayerNotificationSystem.Broadcast(Server.CloseConnection, Channel.External);
}

// Join as a client
else
{
    Client client = new Client();
    client.Connect(args[0], 2048);
}


