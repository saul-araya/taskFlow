using taskFlow.auth.Domain.Enums;

namespace taskFlow.auth.Domain.Exceptions.Messages;

public static class DomainExceptionMessages
{
    public const string InvalidPassword = $"Password is required when the provider is: {nameof(AuthProvider.LOCAL)}";
}
