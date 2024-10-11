using ApplesToApples.Cards;
using ApplesToApples.Networking;
using ApplesToApples.Players.IO;
using ApplesToApples.Utilities;

namespace ApplesToApples.Players;

/// <summary>
/// Controls a player pawn in Apples to Apples as a human player.
/// </summary>
public class HumanController : IPlayerController
{
    public PlayerPawn Pawn { get; }
    public readonly ClientIO IO;

    public HumanController(ClientIO io, PlayerPawn pawn)
    {
        Pawn = pawn;
        IO = io;

        pawn.OnCardReceived += (card) => IO.WriteLine($"You received a new card: {card}");
        pawn.OnPointReceived += _ => IO.WriteLine($"You've been awarded a point");
        pawn.OnPointReceived += _ => IO.WriteLine($"You currently have {pawn.Points} points");
    }

    public async Task<RedApple> Play(GreenApple greenApple)
    {
        var hand = Pawn.Hand;
        IO.WriteLine("Choose a card to play");
        for (int i = 0; i < hand.Length; i++)
        {
            IO.WriteLine($"{i} {hand[i]}");
        }
        
        int index = await IO.ReadAsync(0, hand.Length-1, "Play card number: ");
        
        IO.WriteLine("Waiting for other players...");
        return (RedApple)Pawn.RemoveCard(index);
    }

    public async Task<(IPlayerController, RedApple)> Judge(List<(IPlayerController, RedApple)> submissions, GreenApple greenApple)
    {
        IO.WriteLine("Choose which Red Apple wins");
        for (int i = 0; i < submissions.Count; i++)
        {
            IO.WriteLine($"{i} {submissions[i].Item2}");
        }
        
        int index = await IO.ReadAsync(0, submissions.Count-1, "Winner is card number: ");

        return submissions[index];
    }
}