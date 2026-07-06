namespace Taskly.SharedKernel.Common
{
    // Error object used in Result
    public sealed class Error
    {
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
        public static readonly Error NullValue = new("General.Null", "Null value was provided", ErrorType.Failure);

        private Error(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        public string Code { get; }
        public string Description { get; }
        public ErrorType Type { get; }

        // Factory methods for common error types
        public static Error Failure(string code, string description) =>  new(code, description, ErrorType.Failure);
        public static Error BadRequest(string code, string description) => new(code, description, ErrorType.BadRequest);
        public static Error NotFound(string code, string description) => new(code, description, ErrorType.NotFound);
        public static Error Conflict(string code, string description) => new(code, description, ErrorType.Conflict);
    }
}