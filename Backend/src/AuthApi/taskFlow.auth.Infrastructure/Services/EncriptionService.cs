
using taskFlow.auth.Application.Interfaces;
namespace taskFlow.auth.Infrastructure.Services;

public class EncriptionService : IEncriptionService
{
    public bool CompareEncryption(string plainText, string hash) => BCrypt.Net.BCrypt.Verify(plainText, hash);

    public string EncryptPassword(string plainText) => BCrypt.Net.BCrypt.HashPassword(plainText);
}
