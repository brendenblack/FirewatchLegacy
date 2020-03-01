using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace Blackbox.Firewatch.Application.Features
{
    /// <summary>
    /// A wrapper object that indicates the success or failure of a given operation.
    /// </summary>
    public class Result
    {
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// In the case of failure, contains a message to help diagnose the cause of failure.
        /// </summary>
        public string Message { get; private set; }

        protected Result(bool success, string error)
        {
            Contract.Requires(success || !string.IsNullOrEmpty(error));
            Contract.Requires(!success || string.IsNullOrEmpty(error));

            IsSuccess = success;
            Message = error;
        }

        public static Result Fail(string message)
        {
            return new Result(false, message);
        }

        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(default(T), false, message);
        }

        public static Result Ok()
        {
            return new Result(true, String.Empty);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, String.Empty);
        }

        public static Result Combine(params Result[] results)
        {
            foreach (Result result in results)
            {
                if (!result.IsSuccess)
                {
                    return result;
                }
            }

            return Ok();
        }
    }
}
