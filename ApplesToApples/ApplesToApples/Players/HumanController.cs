using ApplesToApples.Cards;
using ApplesToApples.Networking;
using ApplesToApples.Players.IO;
using ApplesToApples.Utilities;

namespace ApplesToApples.Players;

public class HumanController : IPlayerController
{
    public PlayerPawn Pawn { get; }
    private ClientIO _io;

    public HumanController(ClientIO io, PlayerPawn pawn)
    {
        Pawn = pawn;
        _io = io;

        pawn.OnCardReceived += (card) => _io.WriteLine($"You received a new card: {card}");
        pawn.OnPointReceived += _ => _io.WriteLine($"You've been awarded a point");
    }

    public async Task<RedApple> Play(GreenApple greenApple)
    {
        var hand = Pawn.Hand;
        _io.WriteLine(TextFormatter.Title($"The Green Apple is {greenApple}"));
        _io.WriteLine("Choose a card to play");
        for (int i = 0; i < hand.Length; i++)
        {
            _io.WriteLine($"{i} {hand[i]}");
        }
        
        int index = await _io.ReadAsync(0, hand.Length-1, "Play card number: ");
        
        _io.WriteLine("Waiting for other players...");
        return (RedApple)Pawn.RemoveCard(index);
    }

    public async Task<(IPlayerController, RedApple)> Judge(List<(IPlayerController, RedApple)> submissions, GreenApple greenApple)
    {
        _io.WriteLine(TextFormatter.Title($"The Green Apple is {greenApple}"));
        _io.WriteLine("Choose which Red Apple wins");
        for (int i = 0; i < submissions.Count; i++)
        {
            _io.WriteLine($"{i} {submissions[i].Item2}");
        }
        
        int index = await _io.ReadAsync(0, submissions.Count-1, "Winner is card number: ");

        return submissions[index];
    }
}