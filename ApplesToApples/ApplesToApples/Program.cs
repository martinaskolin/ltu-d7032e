
using ApplesToApples.Game.Variations;
using ApplesToApples.Networking;
using ApplesToApples.Players;
using Microsoft.VisualBasic.CompilerServices;

BasicServer server = new BasicServer();
PlayerManager playerManager = new PlayerManager();
server.OnUserConnected += io => playerManager.AddPlayer(new HumanController(io));
//server.OnUserDisconnected += io => 

await server.AcceptClients(IntegerType.FromString(args[0])); // TODO: make robust
StandardGame game = new StandardGame(playerManager);

