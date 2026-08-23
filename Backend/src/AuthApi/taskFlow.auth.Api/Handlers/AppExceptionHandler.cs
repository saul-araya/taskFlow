using Microsoft.AspNetCore.Diagnostics;
using taskFlow.auth.Api.ResponseDtos;

namespace taskFlow.auth.Api.Handlers;

public class AppExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var response = new ErrorResponseDto(
                Id: httpContext.TraceIdentifier,
                Path: httpContext.Request.Path,
                StatusCode: 500,
                AppErrorCode: "TEXT_ERROR",
                Message: exception.Message,
                Errors: [],
                Date: DateTime.UtcNow
            );

        httpContext.Response.StatusCode = 500;

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        
        return true;
    }
}
