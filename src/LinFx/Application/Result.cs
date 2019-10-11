using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace LinFx.Application
{
    public class Result
    {
        public bool Success { get; protected set; }

        public string Message { get; }

        protected Result(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public static Result Ok()
        {
            return new Result(true, null);
        }

        public static Result Fail(string error)
        {
            return new Result(false, error);
        }

        public static Result Fail(ModelStateDictionary modelStates)
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

        protected internal Result(TValue value, bool success, string message)
            : base(success, message)
        {
            Data = value;
        }
    }
}
