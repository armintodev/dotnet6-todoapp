namespace dotnet6_training.Models.OperationResult;

public class ResponseResult : IResponseResult
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

    public ResponseResult StatusCode(ApiStatusCode statusCode)
    {
        if (statusCode == ApiStatusCode.Success)
            this.SuccessStatusCode();
        else
            this.FailedStatusCode();
        return this;
    }

    public ResponseResult WithMessage(string message)
    {
        this.NewMessage(message);
        return this;
    }

    public ResponseResult ToResult()
    {
        return new ResponseResult
        {
            ApiStatusCode = ApiStatusCode.Success,
            Message = this.Message
        };
    }
}

public class ResponseResult<TData> : ResponseResult, IResponseResult<TData> where TData : class
{
    public TData Data { get; set; }
    public ResponseResult(TData data)
    {
        Data = data;
    }

    public new ResponseResult<TData> StatusCode(ApiStatusCode statusCode)
    {
        if (statusCode == ApiStatusCode.Success)
            this.SuccessStatusCode();
        else
            this.FailedStatusCode();
        return this;
    }

    public new ResponseResult<TData> WithMessage(string message)
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

    public new ResponseResult<TData> ToResult()
    {
        return new ResponseResult<TData>(Data)
        {
            ApiStatusCode = this.ApiStatusCode,
            Message = this.Message
        };
    }
}