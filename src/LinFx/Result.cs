using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LinFx;

/// <summary>
/// Represents the result of an operation.
/// </summary>
public partial class Result
{
    public Result() { }

    protected Result(string message)
     : this(200, message)
    { }

    protected Result(int code, string message)
    {
        Code = code;
        Message = message;
    }

    /// <summary>
    /// Code
    /// </summary>
    public int Code { get; set; } = 200;

    /// <summary>
    /// Message
    /// </summary>
    public string? Message { get; protected set; }
}

public class Result<TValue> : Result
{
    public TValue Data { get; set; }

    protected internal Result(TValue value)
        : this(value, 200, string.Empty)
    { }

    protected internal Result(TValue value, string message)
        : this(value, 200, message)
    { }

    protected internal Result(TValue value, int code, string message)
        : base(code, message)
    {
        Data = value;
    }
}

public partial class Result
{
    /// <summary>
    /// 操作成功
    /// </summary>
    /// <returns></returns>
    public static Result Ok(string message = "操作成功!") => new(message);

    /// <summary>
    /// 操作失败
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static Result Failed(string error) => new(400, error);

    /// <summary>
    /// 操作成功
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static Result<T> Ok<T>(T data) => new(data);

    /// <summary>
    /// 操作成功
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="value"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static Result<TValue> Ok<TValue>(TValue value, string message) => new(value, message);

    /// <summary>
    /// 操作失败
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="value"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public static Result Failed<TValue>(TValue value, string error) => new Result<TValue>(value, error);

    /// <summary>
    /// 操作失败
    /// </summary>
    /// <param name="modelStates"></param>
    /// <returns></returns>
    public static Result Failed(ModelStateDictionary modelStates)
    {
        IEnumerable<string>? errors = null;
        if (modelStates != null && !modelStates.IsValid)
        {
            errors = from modelState in modelStates.Values
                     from error in modelState?.Errors
                     select error.ErrorMessage;
        }
        return new Result(400, errors != null ? string.Join("\r\n", errors) : null);
    }

    /// <summary>
    /// NotFound
    /// </summary>
    /// <returns></returns>
    public static Result NotFound(string? message = default)
    {
        if (message is null)
            message = "Not Found!";

        var result = new Result(404, message);
        return result;
    }
}

public partial class Result
{
    public static bool operator ==(Result result1, Result result2)
    {
        return result1.Code == result2.Code;
    }

    public static bool operator !=(Result result1, Result result2)
    {
        return result1.Code != result2.Code;
    }

    public static bool operator true(Result result)
    {
        return result.Code == 200;
    }

    public static bool operator false(Result result)
    {
        return result.Code != 200;
    }
}