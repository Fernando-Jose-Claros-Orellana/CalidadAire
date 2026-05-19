using Microsoft.EntityFrameworkCore;
using ParcialFJCO.Domain.DTO;
using ParcialFJCO.Application.Interface;
using ParcialFJCO.Infraestructure.Data;

namespace ParcialFJCO.Infraestructure.Service
{
    public class AuthService : ILoginService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ITokenService _tokenService;

        public AuthService(AppDbContext appDbContext, ITokenService tokenService)
        {
            _appDbContext = appDbContext;
            _tokenService = tokenService;
        }

        public async Task<ResponseT> Login(LoginUserN requesdt)
        {
            try
            {
                var usuario = await _appDbContext.Usuarios
                    .FirstOrDefaultAsync(u => u.Username == requesdt.Username);
                if (usuario == null)
                {
                    return new ResponseT { Success = false, Message = "Usuario no encontrado" };
                }

                var passwordCorrecta = BCrypt.Net.BCrypt.Verify(requesdt.Password, usuario.PasswordHash);
                if (!passwordCorrecta)
                {
                    return new ResponseT { Success = false, Message = "Contraseña incorrecta" };
                }

                var token = _tokenService.GenerateToken(usuario);
                return new ResponseT { Success = true, Message = token };
            }
            catch (Exception ex)
            {
                return new ResponseT { Success = false, Message = $"Error al iniciar sesión: {ex.Message}" };
            }
        }
    }
}
