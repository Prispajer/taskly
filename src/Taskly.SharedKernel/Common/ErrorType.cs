namespace Taskly.SharedKernel.Common
{
    // Error categories used in Result
    public enum ErrorType
    {
        Failure = 0,
        Validation = 1,
        BadRequest = 2,
        NotFound = 3,
        Conflict = 4
    }
}