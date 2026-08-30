
using taskFlow.auth.Application.Exceptions.Codes;

namespace taskFlow.auth.Application.Exceptions;

public class UnauthorizedException(string message) : ApplicationException(
    ApplicationExceptionCodes.UNAUTHORIZED_CODE,
    message
){}
