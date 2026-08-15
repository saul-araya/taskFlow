
namespace taskFlow.auth.Application.Interfaces;

public interface IEncriptionService
{
    string EncryptPassword(string plainText);
    bool CompareEncryption(string plainText, string hash);
}
