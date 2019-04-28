namespace LinFx.Application.Models
{
    public class Result
    {
        public int Code { get; protected set; } = 200;

        public string Msg { get; protected set; } = "success";

        public Result() { }

        public Result(int code, string msg)
        {
            Code = code;
            Msg = msg;
        }
    }

    public class Result<T> : Result
    {
        public T Date { get; }

        public Result() { }

        public Result(T date)
        {
            Date = date;
        }

        public Result(int code, string msg, T date)
            : this(date)
        {
            Code = code;
            Msg = msg;
        }
    }
}
