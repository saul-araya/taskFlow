namespace taskFlow.auth.Api.ResponseDtos;

public record ErrorResponseDto(
    string Id,
    string Path,
    int StatusCode,
    string AppErrorCode,
    string Message,
    List<string> Errors,
    DateTime Date
){}
