using System.Net;

namespace softrobotics.auth.application.Common.Model;

public class Result
{
    internal Result(bool isSuccess = false, int statusCode = (int)HttpStatusCode.OK)
    {
        IsSuccess = isSuccess;
        StatusCode = statusCode;
    }

    internal Result(IEnumerable<string> errors, int statusCode = (int)HttpStatusCode.OK)
    {
        Errors = errors;
        IsSuccess = false;
        StatusCode = statusCode;
    }

    public int StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public IEnumerable<string> Errors { get; set; }

    public static Result Success(int statusCode = (int)HttpStatusCode.OK) => new(true, statusCode);
    public static Result Failure(int statusCode = (int)HttpStatusCode.OK) => new(false, statusCode);
    public static Result Failure(IEnumerable<string> errors, int statusCode = (int)HttpStatusCode.OK) => new(errors, statusCode);
}

public class Result<T>
{
    internal Result(bool isSuccess = false, int statusCode = (int)HttpStatusCode.OK)
    {
        IsSuccess = isSuccess;
        StatusCode = statusCode;
    }

    internal Result(IEnumerable<string> errors, int statusCode = (int)HttpStatusCode.OK)
    {
        Errors = errors;
        IsSuccess = false;
        StatusCode = statusCode;
    }

    internal Result(T data, int statusCode = (int)HttpStatusCode.OK)
    {
        Data = data;
        IsSuccess = true;
        StatusCode = statusCode;
    }

    public int StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public IEnumerable<string> Errors { get; set; }
    public T Data { get; set; }

    public static Result<T> Success(bool isSuccess = false, int statusCode = (int)HttpStatusCode.OK) => new(isSuccess, statusCode);
    public static Result<T> Success(T data, int statusCode = (int)HttpStatusCode.OK) => new(data, statusCode);
    public static Result<T> Failure(IEnumerable<string> errors, int statusCode = (int)HttpStatusCode.OK) => new(errors, statusCode);
}