
namespace taskFlow.auth.Application.Dtos.Token;

public record RefreshTokenDto(
    string token,
    string RefreshTokenHash,
    DateTime CreatedAt,
    DateTime ExpiresAt,
    bool IsActive
){}
