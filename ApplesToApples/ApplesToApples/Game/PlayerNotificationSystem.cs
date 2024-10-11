using ApplesToApples.Networking;
using ApplesToApples.Players;

namespace ApplesToApples.Game;

/// <summary>
/// Responsible for notifying human players of events in the game.
/// </summary>
public static class PlayerNotificationSystem
{
    
    private static Dictionary<Channel, Action<string>> _channels = new Dictionary<Channel, Action<string>>();
    
    
    
    /// <summary>
    /// Try to subscribe a player to a channel. If the player is not a human player, ignore.
    /// </summary>
    public static void TrySubscribePlayer(Channel channel, IPlayerController player)
    {
        try
        {
            Subscribe(channel, ((HumanController)player).IO.WriteLine);
        }
        catch
        {
            // ignored
        }
    }
    
    /// <summary>
    /// Clear subscriptions to a channel.
    /// </summary>
    /// <param name="channel"></param>
    public static void ClearSubscriptions(Channel channel)
    {
        try
        {
            _channels[channel].GetInvocationList().ToList().ForEach(del => _channels[channel] -= (Action<string>)del);
        }
        catch
        {
            // ignored
        }
    }
    
    public static void Subscribe(Channel channel, Action<string>? subscriber)
    { 
        if (!_channels.TryAdd(channel, subscriber)) _channels[channel] += subscriber;
    }
    
    private static void Unsubscribe(Channel channel, Action<string>? subscriber)
    {
        if (_channels.ContainsKey(channel)) _channels[channel] -= subscriber;
    }

    /// <summary>
    /// Broadcasts a message to all human players subscribed to a channel.
    /// </summary>
    /// <param name="msg">message to broadcast</param>
    /// <param name="channel">channel to broadcast on</param>
    public static void Broadcast(string msg, Channel channel)
    {
        try
        {
            _channels[channel]?.Invoke(msg);
        }
        catch
        {
            // ignored
        }
    }
    
}

public enum Channel
{
    All,
    External,
    Local,
    Judge,
    Submitters
}