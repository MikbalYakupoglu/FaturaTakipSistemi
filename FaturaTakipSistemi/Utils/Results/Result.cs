namespace FaturaTakip.Utils.Results;

public class Result : IResult
{
    public Result(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    public Result(bool isSuccess,string message) : this(isSuccess)
    {
        Message = message;
    }

    public bool IsSuccess { get; }
    public string Message { get; }
}