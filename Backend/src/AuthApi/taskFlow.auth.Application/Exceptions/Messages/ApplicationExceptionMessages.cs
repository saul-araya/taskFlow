namespace taskFlow.auth.Application.Exceptions.Messages;

public static class ApplicationExceptionMessages
{
    public const string InvalidData = "The {0} is requiered, it cannot be null or empty.";
    public const string NotFound = "Requested element not found";
    public const string InvalidCredentials = "Invalid user credentials";
    public const string UnauthorizedRefreshToken = "Invalid or revoke refresh token";
    public const string UnauthorizedGoogleIdToken = "Invalid google user ID token";
}
