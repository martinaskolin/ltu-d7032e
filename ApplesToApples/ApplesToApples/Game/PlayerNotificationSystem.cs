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
    /// Subscribes a player to a channel.
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="subscriber"></param>
    public static void Subscribe(Channel channel, Action<string> subscriber)
    { 
        if (!_channels.TryAdd(channel, subscriber)) _channels[channel] += subscriber;
    }

    /// <summary>
    /// Unsubscribes a player from a channel.
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="subscriber"></param>
    public static void Unsubscribe(Channel channel, Action<string> subscriber)
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
    External
}