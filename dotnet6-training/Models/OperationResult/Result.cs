namespace dotnet6_training.Models.OperationResult;

public class Result : IResult
{
    public ApiStatusCode ApiStatusCode { get; set; }
    public string Message { get; set; }

    public void FailedStatusCode()
    {
        this.ApiStatusCode = ApiStatusCode.Failed;
    }

    public void SuccessStatusCode()
    {
        this.ApiStatusCode = ApiStatusCode.Success;
    }

    public void NewMessage(string message)
    {
        this.Message = message;
    }

    public Result StatusCode(ApiStatusCode statusCode)
    {
        if (statusCode == ApiStatusCode.Success) this.SuccessStatusCode();
        else this.FailedStatusCode();
        return this;
    }

    public Result WithMessage(string message)
    {
        this.NewMessage(message);
        return this;
    }

    public Result ToResult()
    {
        return new Result
        {
            ApiStatusCode = ApiStatusCode.Success,
            Message = this.Message
        };
    }
}

public class Result<TData> : Result, IResult<TData> where TData : class
{
    public TData Data { get; set; }
    public Result(TData data)
    {
        Data = data;
    }

    public new Result<TData> StatusCode(ApiStatusCode statusCode)
    {
        if (statusCode == ApiStatusCode.Success) this.SuccessStatusCode();
        else this.FailedStatusCode();
        return this;
    }

    public new Result<TData> WithMessage(string message)
    {
        this.NewMessage(message);
        return this;
    }

    //public new IResult<TData> ToResult()
    //{
    //    return new Result<TData>(Data)
    //    {
    //        ApiStatusCode = this.ApiStatusCode,
    //        Message = this.Message
    //    };
    //}

    public new Result<TData> ToResult()
    {
        return new Result<TData>(Data)
        {
            ApiStatusCode = this.ApiStatusCode,
            Message = this.Message
        };
    }
}