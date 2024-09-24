using ApplesToApples.Game;
using ApplesToApples.Game.Variations;
using ApplesToApples.Networking;
using ApplesToApples.Players;
using ApplesToApples.Utilities;

List<IPlayerController> controllers = new List<IPlayerController>();

LocalIO host = new LocalIO();
controllers.Add(new HumanController(host, new PlayerPawn()));
GameEventHandler.Subscribe("ALL", host.WriteLine);

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
        GameEventHandler.Subscribe("ALL", io.WriteLine);
    };
    server.OnUserConnected += _ => GameEventHandler.Broadcast("Player connected", "ALL");
    
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
}

// Join as a client
else
{
    Client client = new Client();
    client.Connect(args[0], 2048);
}


