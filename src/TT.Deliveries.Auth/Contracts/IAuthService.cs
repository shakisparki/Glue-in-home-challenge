using TT.Deliveries.Auth.Models;

namespace TT.Deliveries.Auth.Services.Contracts
{
    public interface IAuthService
    {
        ServicePrincipal Authenticate(string username, string pwd);
    }
}