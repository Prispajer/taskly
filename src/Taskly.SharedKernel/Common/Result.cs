namespace Taskly.SharedKernel.Common
{
    // Base result type for operations (success/failure + error info)
    public class Result
    {
        public bool IsSuccess { get; }
        public Error Error { get; }

        protected Result(bool isSuccess, Error error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        // Success result without value
        public static Result Success() => new(true, Error.None);

        // Failure result with error
        public static Result Failure(Error error) => new(false, error);

        // Success result with value
        public static Result<T> Success<T>(T value) => new(value, true, Error.None);

        // Failure result with error and generic type
        public static Result<T> Failure<T>(Error error) => new(default, false, error);

    }

    // Generic result type with value
    public class Result<T> : Result
    {
        private readonly T? _value;
        
        public Result(T? value, bool isSuccess, Error error) : base(isSuccess, error)
        {
            _value = value;
        }
        
        // Returns value if success, throws if failure
        public T Value => IsSuccess ? _value! :  throw new InvalidOperationException("Cannot get a value.");
    }
}