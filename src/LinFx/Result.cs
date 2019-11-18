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
        public Result() { }

        protected Result(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        /// <summary>
        /// Flag indicating whether if the operation succeeded or not.
        /// </summary>
        /// <value>True if the operation succeeded, otherwise false.</value>
        public bool Success { get; protected set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; protected set; }

        public static Result Ok() => new Result(true, "操作成功");

        public static Result<T> Ok<T>(T data)
        {
            return new Result<T>(data);
        }

        public static Result<TValue> Ok<TValue>(TValue value, string message)
        {
            return new Result<TValue>(value, true, message);
        }

        public static Result Failed(string error)
        {
            return new Result(false, error);
        }

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

        public static Result<TValue> Failed<TValue>(string error)
        {
            return new Result<TValue>(default, false, error);
        }

        public static Result Failed<TValue>(TValue value, string error)
        {
            return new Result<TValue>(value, false, error);
        }
    }
}
