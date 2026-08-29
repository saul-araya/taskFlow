
using taskFlow.auth.Application.Exceptions.Codes;

namespace taskFlow.auth.Application.Exceptions;

public class InvalidCredentialsException(string message) : ApplicationException(
    ApplicationExceptionCodes.INVALID_CREDENTIALS_CODE,
    message
)
{
}
