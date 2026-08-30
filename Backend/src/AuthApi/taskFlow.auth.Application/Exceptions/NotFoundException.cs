
using taskFlow.auth.Application.Exceptions.Codes;

namespace taskFlow.auth.Application.Exceptions;

public class NotFoundException(string message) : ApplicationException(
    ApplicationExceptionCodes.NOT_FOUND_CODE,
    message
){}
