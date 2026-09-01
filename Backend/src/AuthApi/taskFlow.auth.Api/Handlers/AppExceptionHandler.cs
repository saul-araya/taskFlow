using Microsoft.AspNetCore.Diagnostics;
using taskFlow.auth.Api.ResponseDtos;
using taskFlow.auth.Application.Exceptions;
using taskFlow.auth.Domain.Exceptions;

namespace taskFlow.auth.Api.Handlers;

public class AppExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var data = GetErrorData(exception);

        var response = new ErrorResponseDto(
                Id: httpContext.TraceIdentifier,
                Path: httpContext.Request.Path,
                StatusCode: data.StatusCode,
                AppErrorCode: data.AppErrorCode,
                Message: data.Message,
                Errors: data.Errors,
                Date: DateTime.UtcNow
            );

        httpContext.Response.StatusCode = data.StatusCode;

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        
        return true;
    }

    private DataErrorRespondeDto GetErrorData(Exception exception)
    {
        return exception switch
        {
            InvalidCredentialsException ex =>
                BuildErrorData(StatusCodes.Status401Unauthorized, ex.code, ex.Message),

            NotFoundException ex =>
                BuildErrorData(StatusCodes.Status404NotFound, ex.code, ex.Message),

            BadRequestDataException ex =>
                BuildErrorData(StatusCodes.Status404NotFound, ex.code, ex.Message),

            InvalidPasswordException ex =>
                BuildErrorData(StatusCodes.Status400BadRequest, ex.code, ex.Message),

            UnauthorizedException ex =>
                BuildErrorData(StatusCodes.Status401Unauthorized, ex.code, ex.Message),

            _ =>
                BuildErrorData(StatusCodes.Status400BadRequest, "APP_EXCEPTION", exception.Message)
        };
    }

    private DataErrorRespondeDto BuildErrorData(
        int StatusCode,
        string AppErrorCode,
        string Message,
        List<string>? Errors = null
    ){
        return new DataErrorRespondeDto(
            StatusCode,
            AppErrorCode,
            Message,
            Errors ?? []
        );
    }
}
