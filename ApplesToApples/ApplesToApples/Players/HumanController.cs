using ApplesToApples.Cards;
using ApplesToApples.Networking;

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
        _io.WriteLine($"The Green Apple is {greenApple}");
        _io.WriteLine("Choose a card to play");
        for (int i = 0; i < hand.Length; i++)
        {
            _io.WriteLine($"{i} {hand[i]}");
        }

        int index = await _io.ReadAsync(0, hand.Length, "Play card number ");
        
        _io.WriteLine("Waiting for other players...");
        return (RedApple)Pawn.RemoveCard(index);
    }

    public async Task<(IPlayerController, RedApple)> Judge(List<(IPlayerController, RedApple)> submissions, GreenApple greenApple)
    {
        _io.WriteLine($"Current green apple is {greenApple}");
        _io.WriteLine("Choose which red apple wins");
        for (int i = 0; i < submissions.Count; i++)
        {
            _io.WriteLine($"{i} {submissions[i].Item2}");
        }

        int index = await _io.ReadAsync(0, submissions.Count, "Winner is card number ");

        return submissions[index];
    }
}