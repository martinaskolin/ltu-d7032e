namespace ApplesToApples.Phases;

public interface IResult<out T>
{
    public T Result();
}