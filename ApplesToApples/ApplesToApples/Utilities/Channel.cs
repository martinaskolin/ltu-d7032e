namespace ApplesToApples.Utilities;

public class Channel<T>
{
    private event Action<T> _onBroadcast;
    
    public void Subscribe(Action<T> action)
    {
        _onBroadcast += action;
    }
    
    public void Unsubscribe(Action<T> action)
    {
        _onBroadcast -= action;
    }

    public void Broadcast(T message)
    {
        _onBroadcast?.Invoke(message);
    }
}