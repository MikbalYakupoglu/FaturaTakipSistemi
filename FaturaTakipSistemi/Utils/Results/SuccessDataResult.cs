namespace FaturaTakip.Utils.Results;

public class SuccessDataResult<T> : DataResult<T>
{
    public SuccessDataResult(T data) : base(true, data)
    {
    }

    public SuccessDataResult(T data, string message) : base(true, data, message)
    {
    }
}