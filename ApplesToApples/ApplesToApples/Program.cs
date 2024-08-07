using ApplesToApples.Game.Variations;
using ApplesToApples.Networking;
using ApplesToApples.Players;

//Server server = new Server("localhost", 2048);
PlayerManager playerManager = new PlayerManager();
//playerManager.AddPlayer(new HumanController(new LocalIO())); // Add local player
//server.OnUserConnected += io => playerManager.AddPlayer(new HumanController(io)); // Add online players

//await server.AcceptConnectionsAsync(IntegerType.FromString(args[0])); // TODO: make robust
//await server.AcceptConnectionsAsync(1);
for (int i = 0; i < 7; i++)
{
    playerManager.AddPlayer(new BotController());
}

StandardGame game = new StandardGame(playerManager);

bool done = false;
while (!done)
{
    done = !await game.Step();
}
