using dotnet6_training.Models.Enums;

namespace dotnet6_training.Models.OperationResult;

public interface IResult
{
    ApiStatusCode ApiStatusCode { get; protected set; }
    public string Message { get; protected set; }

    void SuccessStatusCode();
    void FailedStatusCode();
    void NewMessage(string message);

    public IResult StatusCode(ApiStatusCode statusCode);

    public IResult WithMessage(string message);

    public IResult ToResult();
}

public interface IResult<out TData> : IResult where TData : class
{
    public TData Data { get; }

    public new IResult<TData> StatusCode(ApiStatusCode statusCode);

    public new IResult<TData> WithMessage(string message);

    public new IResult<TData> ToResult();
}