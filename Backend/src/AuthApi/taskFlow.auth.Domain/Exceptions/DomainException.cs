
namespace taskFlow.auth.Domain.Exceptions;

public class DomainException : Exception
{
    public string code;
    public DomainException(string code, string message) : base(message) => this.code = code;
}
