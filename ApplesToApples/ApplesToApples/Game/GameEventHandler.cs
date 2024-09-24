using ApplesToApples.Networking;

namespace ApplesToApples.Game;

public class GameEventHandler
{
    
    private static Dictionary<string, Action<string>> _channels = new Dictionary<string, Action<string>>();

    public static void Subscribe(string channel, Action<string> subscriber)
    { 
        if (!_channels.TryAdd(channel.ToUpper(), subscriber)) _channels[channel.ToUpper()] += subscriber;
    }

    public static void Unsubscribe(string channel, Action<string> subscriber)
    {
        if (_channels.ContainsKey(channel.ToUpper())) _channels[channel.ToUpper()] -= subscriber;
    }

    public static void Broadcast(string msg, string channel)
    {
        _channels[channel.ToUpper()]?.Invoke(msg);
    }
}