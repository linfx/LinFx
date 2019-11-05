namespace LinFx
{
    public class Result<TValue> : Result
    {
        public TValue Data { get; set; }

        protected internal Result(TValue data)
        {
            Succeeded = true;
            Data = data;
        }

        protected internal Result(TValue value, bool success, string message)
            : base(success, message)
        {
            Data = value;
        }
    }
}
