﻿namespace LinFx
{
    public class Result<TValue> : Result
    {
        public TValue Data { get; set; }

        protected internal Result(TValue data)
        {
            _code = 200;
            Succeeded = true;
            Data = data;
            Message = "操作成功";
        }

        protected internal Result(TValue value, bool success, string message)
            : base(success, message)
        {
            Data = value;
        }
    }
}