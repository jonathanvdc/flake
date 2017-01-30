using System;

namespace Flake
{
    /// <summary>
    /// A value that is either a result or an error.
    /// </summary>
    public struct ResultOrError<TResult, TError>
    {
        private ResultOrError(TResult Result)
        {
            this = default(ResultOrError<TResult, TError>);
            this.result = Result;
            this.IsError = false;
        }

        private ResultOrError(TError Error)
        {
            this = default(ResultOrError<TResult, TError>);
            this.error = Error;
            this.IsError = true;
        }

        private TResult result;
        private TError error;

        /// <summary>
        /// Gets the result if this instance represents a result; 
        /// otherwise, the default value.
        /// </summary>
        /// <value>
        /// The result if this instance represents a result; 
        /// otherwise, the default value.
        /// </value>
        public TResult ResultOrDefault { get { return result; } }

        /// <summary>
        /// Gets the error if this instance represents an error; 
        /// otherwise, the default value.
        /// </summary>
        /// <value>
        /// The error if this instance represents an error; 
        /// otherwise, the default value.
        /// </value>
        public TError ErrorOrDefault { get { return error; } }

        /// <summary>
        /// Gets a value indicating whether this instance represents an error.
        /// </summary>
        /// <value><c>true</c> if this instance represents an error; otherwise, <c>false</c>.</value>
        public bool IsError { get; private set; }

        /// <summary>
        /// Applies the given mapping to this instance's result, if it represents
        /// a result; otherwise, an error equivalent to this instance is returned.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="Map">The mapping function.</param>
        /// <typeparam name="T">The type of the new result.</typeparam>
        public ResultOrError<T, TError> BindResult<T>(Func<TResult, ResultOrError<T, TError>> Map)
        {
            if (!IsError)
                return Map(result);
            else
                return new ResultOrError<T, TError>(error);
        }

        /// <summary>
        /// Applies the given mapping to this instance's error, if it represents
        /// an error; otherwise, a result equivalent to this instance is returned.
        /// </summary>
        /// <returns>The error.</returns>
        /// <param name="Map">The mapping function.</param>
        /// <typeparam name="T">The type of the new error.</typeparam>
        public ResultOrError<TResult, T> BindError<T>(Func<TError, ResultOrError<TResult, T>> Map)
        {
            if (IsError)
                return Map(error);
            else
                return new ResultOrError<TResult, T>(result);
        }

        /// <summary>
        /// Applies the given mapping to this instance's result, if it represents
        /// a result; otherwise, an error equivalent to this instance is returned.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="Map">The mapping function.</param>
        /// <typeparam name="T">The type of the new result.</typeparam>
        public ResultOrError<T, TError> MapResult<T>(Func<TResult, T> Map)
        {
            if (!IsError)
                return new ResultOrError<T, TError>(Map(result));
            else
                return new ResultOrError<T, TError>(error);
        }

        /// <summary>
        /// Applies the given mapping to this instance's error, if it represents
        /// an error; otherwise, a result equivalent to this instance is returned.
        /// </summary>
        /// <returns>The error.</returns>
        /// <param name="Map">The mapping function.</param>
        /// <typeparam name="T">The type of the new error.</typeparam>
        public ResultOrError<TResult, T> MapError<T>(Func<TError, T> Map)
        {
            if (IsError)
                return new ResultOrError<TResult, T>(Map(error));
            else
                return new ResultOrError<TResult, T>(result);
        }

        /// <summary>
        /// Creates a result from the given value.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="Result">The result's value.</param>
        public static ResultOrError<TResult, TError> CreateResult(TResult Result)
        {
            return new ResultOrError<TResult, TError>(Result);
        }

        /// <summary>
        /// Creates an error from the given value.
        /// </summary>
        /// <returns>The error.</returns>
        /// <param name="Error">The error's value.</param>
        public static ResultOrError<TResult, TError> CreateError(TError Error)
        {
            return new ResultOrError<TResult, TError>(Error);
        }
    }
}

