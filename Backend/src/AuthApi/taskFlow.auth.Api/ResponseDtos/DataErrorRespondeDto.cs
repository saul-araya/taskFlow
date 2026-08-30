namespace taskFlow.auth.Api.ResponseDtos;

public record DataErrorRespondeDto(
    int StatusCode,
    string AppErrorCode,
    string Message,
    List<string> Errors
){}
