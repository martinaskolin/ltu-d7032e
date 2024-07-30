using ApplesToApples.Game.Variations;
using ApplesToApples.Networking;
using ApplesToApples.Players;

Server server = new Server("localhost", 2048);
PlayerManager playerManager = new PlayerManager();
playerManager.AddPlayer(new HumanController(new LocalIO())); // Add local player
server.OnUserConnected += io => playerManager.AddPlayer(new HumanController(io)); // Add online players

//await server.AcceptConnectionsAsync(IntegerType.FromString(args[0])); // TODO: make robust
await server.AcceptConnectionsAsync(1);

StandardGame game = new StandardGame(playerManager);

