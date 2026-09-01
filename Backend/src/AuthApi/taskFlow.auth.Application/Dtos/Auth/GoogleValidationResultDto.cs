
namespace taskFlow.auth.Application.Dtos.Auth;

public record GoogleValidationResultDto(
    bool IsSuccesfull,
    string? Name,
    string? DisplayName,
    string? Email,
    string? ImageLink,
    string? ProviderUserId
){}
