namespace Taskly.SharedKernel
{
    // Base result type for operations (success/failure + error info)
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        public Result(bool isSuccess, Error error)
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

        internal Result(T? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        // Returns value if success, throws if failure
        public T Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("Cannot get the value.");

        // Implicit conversion from value to Result<T>
        public static implicit operator Result<T>(T? value) =>
            value is not null
                ? Success(value)
                : Failure<T>(Error.NullValue);
    }
}