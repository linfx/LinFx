using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace LinFx
{
    /// <summary>
    /// Represents the result of an operation.
    /// </summary>
    public class Result
    {
        protected int _code = 400;

        public Result() { }

        protected Result(bool success, string message)
        {
            Succeeded = success;
            Message = message;
        }

        /// <summary>
        /// Code
        /// </summary>
        public int Code 
        {
            get
            {
                if (Succeeded && _code != 200)
                    _code = 200;
                return _code;
            }
            set { _code = value; }
        }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        /// Flag indicating whether if the operation succeeded or not.
        /// </summary>
        /// <value>True if the operation succeeded, otherwise false.</value>
        public bool Succeeded { get; protected set; }

        /// <summary>
        /// 操作成功
        /// </summary>
        /// <returns></returns>
        public static Result Ok() => new Result(true, "操作成功");

        /// <summary>
        /// 操作成功
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Result<T> Ok<T>(T data) => new Result<T>(data);

        /// <summary>
        /// 操作成功
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Result<TValue> Ok<TValue>(TValue value, string message) => new Result<TValue>(value, true, message);

        /// <summary>
        /// 操作失败
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static Result Failed(string error) => new Result(false, error);

        /// <summary>
        /// 操作失败
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="error"></param>
        /// <returns></returns>
        public static Result<TValue> Failed<TValue>(string error) => new Result<TValue>(default, false, error);

        /// <summary>
        /// 操作失败
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static Result Failed<TValue>(TValue value, string error) => new Result<TValue>(value, false, error);

        /// <summary>
        /// 操作失败
        /// </summary>
        /// <param name="modelStates"></param>
        /// <returns></returns>
        public static Result Failed(ModelStateDictionary modelStates)
        {
            IEnumerable<string> errors = null;
            if (modelStates != null && !modelStates.IsValid)
            {
                errors = from modelState in modelStates.Values
                         from error in modelState?.Errors
                         select error.ErrorMessage;
            }
            return new Result(false, errors != null ? string.Join("\r\n", errors) : null);
        }

    }

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
