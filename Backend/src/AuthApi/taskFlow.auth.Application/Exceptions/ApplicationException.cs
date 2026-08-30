
namespace taskFlow.auth.Application.Exceptions;

public class ApplicationException : Exception
{
    public string code;
    public ApplicationException(string code, string message) : base(message) => this.code = code;
}
