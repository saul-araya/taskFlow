
using taskFlow.auth.Application.Dtos.Auth;
using taskFlow.auth.Application.Dtos.User;
using taskFlow.auth.Application.Interfaces;
using taskFlow.auth.Domain.Entities;

namespace taskFlow.auth.Infrastructure.Services;

public class GoogleAuthenticationService : IAuthProvider<GoogleAuthRequestsDto>
{
    public async Task<AuthResultDto> Authenticate(GoogleAuthRequestsDto dto)
    {
        throw new NotImplementedException();
    }
}
