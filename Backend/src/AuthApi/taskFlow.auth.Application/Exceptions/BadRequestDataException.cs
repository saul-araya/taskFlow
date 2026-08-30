
using taskFlow.auth.Application.Exceptions.Codes;

namespace taskFlow.auth.Application.Exceptions;

public class BadRequestDataException(string message) : ApplicationException(
    ApplicationExceptionCodes.INVALID_DATA_CODE,
    message
){}
