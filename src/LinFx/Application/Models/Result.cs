namespace LinFx.Application.Models
{
    public class Result
    {
        public int Code { get; protected set; } = 200;

        public string Message { get; protected set; } = "success";

        public Result() { }

        public Result(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }

    public class Result<T> : Result
    {
        public T Data { get; }

        public Result() { }

        public Result(T data)
        {
            Data = data;
        }

        public Result(int code, string message, T data)
            : this(data)
        {
            Code = code;
            Message = message;
        }
    }
}
