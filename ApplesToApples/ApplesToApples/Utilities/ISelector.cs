namespace ApplesToApples.Utilities;

public interface ISelector<T>
{
    public Task<T> SelectElement(List<T> elements);
}