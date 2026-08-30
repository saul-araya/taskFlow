
using taskFlow.auth.Domain.Exceptions.Codes;

namespace taskFlow.auth.Domain.Exceptions;

public class InvalidPasswordException(string message) : DomainException(
    DomainExceptionCodes.INVALID_PASSWORD,
    message
)
{}
