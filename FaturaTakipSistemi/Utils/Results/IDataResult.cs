namespace FaturaTakip.Utils.Results;

public interface IDataResult<T> : IResult
{
    T Data { get; }
}