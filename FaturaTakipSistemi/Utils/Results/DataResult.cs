namespace FaturaTakip.Utils.Results;

public class DataResult<T> : Result, IDataResult<T>
{
    public DataResult(bool isSuccess, T data) : base(isSuccess)
    {
        Data = data;
    }

    public DataResult(bool isSuccess, T data, string message) : base(isSuccess, message)
    {
        Data = data;
    }

    public T Data { get; }
}