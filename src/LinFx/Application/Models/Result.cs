using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace LinFx.Application.Models
{
    /// <summary>
    /// Represents the result of an operation.
    /// </summary>
    public class Result
    {
        private readonly List<Error> _errors = new List<Error>();

        public Result() { }

        protected Result(bool success, string message)
        {
            Succeeded = success;
            Message = message;
        }

        /// <summary>
        /// Flag indicating whether if the operation succeeded or not.
        /// </summary>
        /// <value>True if the operation succeeded, otherwise false.</value>
        public bool Succeeded { get; protected set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        /// An <see cref="IEnumerable{T}"/> of <see cref="Error"/>s containing an errors
        /// that occurred during the identity operation.
        /// </summary>
        /// <value>An <see cref="IEnumerable{T}"/> of <see cref="Error"/>s.</value>
        public IEnumerable<Error> Errors => _errors;

        /// <summary>
        /// Returns an <see cref="Result"/> indicating a successful operation.
        /// </summary>
        /// <returns>An <see cref="Result"/> indicating a successful operation.</returns>
        public static Result Success { get; } = new Result { Succeeded = true };

        /// <summary>
        /// Creates an <see cref="Result"/> indicating a failed identity operation, with a list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="Error"/>s which caused the operation to fail.</param>
        /// <returns>An <see cref="Result"/> indicating a failed identity operation, with a list of <paramref name="errors"/> if applicable.</returns>
        public static Result Failed(params Error[] errors)
        {
            var result = new Result { Succeeded = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }
            return result;
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

        public static Result<TValue> Fail<TValue>(string error)
        {
            return new Result<TValue>(default, false, error);
        }

        public static Result Failed<TValue>(TValue value, string error)
        {
            return new Result<TValue>(value, false, error);
        }

        public static Result Ok() => Success;

        public static Result<T> Ok<T>(T data)
        {
            return new Result<T>(data);
        }

        public static Result<TValue> Ok<TValue>(TValue value, string message)
        {
            return new Result<TValue>(value, true, message);
        }
    }
}
