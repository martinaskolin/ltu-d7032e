using ApplesToApples.Game.Variations;
using ApplesToApples.Networking;
using ApplesToApples.Players;
using ApplesToApples.Utilities;

//Server server = new Server("localhost", 2048);
List<IPlayerController> controllers = new List<IPlayerController>();
controllers.Add(new HumanController(new LocalIO(), new PlayerPawn())); // Add local player

//server.OnUserConnected += io => playerManager.AddPlayer(new HumanController(io)); // Add online players

//await server.AcceptConnectionsAsync(IntegerType.FromString(args[0])); // TODO: make robust
//await server.AcceptConnectionsAsync(1);
for (int i = 0; i < 6; i++)
{
    controllers.Add(new BotController(new PlayerPawn()));
}

StandardGame game = new StandardGame(controllers);

bool done = false;
while (!done)
{
    done = !await game.Step();
}
