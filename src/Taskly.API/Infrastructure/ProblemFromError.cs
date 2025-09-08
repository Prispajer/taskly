using Taskly.SharedKernel;

namespace Taskly.API.Infrastructure
{
    // Converts a domain-level Error object into an HTTP Problem response
    public static class ProblemResults
    {
        public static IResult FromError(Error error, int statusCode = StatusCodes.Status400BadRequest)
        {
            return Results.Problem(
                title: error.Code,           // Short error code (e.g. "ValidationError")
                detail: error.Description,   // Human-readable error message
                statusCode: statusCode       // HTTP status code (default: 400 Bad Request)
            );
        }
    }
}
