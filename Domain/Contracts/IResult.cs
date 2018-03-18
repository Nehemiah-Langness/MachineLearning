namespace Domain.Contracts
{
    public interface IResult<in T>
    {
        IResult<T> Heuristic(T scenario);
    }
}