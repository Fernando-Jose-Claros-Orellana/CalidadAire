using ParcialFJCO.Domain.Entities;

namespace ParcialFJCO.Application.Interface
{
    public interface ITokenService
    {
        string GenerateToken(Usuario usuario);
    }
}
