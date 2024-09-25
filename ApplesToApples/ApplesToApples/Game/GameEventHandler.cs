using ApplesToApples.Networking;

namespace ApplesToApples.Game;

public class GameEventHandler
{
    
    private static Dictionary<Channel, Action<string>> _channels = new Dictionary<Channel, Action<string>>();

    public static void Subscribe(Channel channel, Action<string> subscriber)
    { 
        if (!_channels.TryAdd(channel, subscriber)) _channels[channel] += subscriber;
    }

    public static void Unsubscribe(Channel channel, Action<string> subscriber)
    {
        if (_channels.ContainsKey(channel)) _channels[channel] -= subscriber;
    }

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