using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ParcialFJCO.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProtectedController : ControllerBase
    {
        [HttpGet]
        [Route("me")]
        public IActionResult GetCurrentUser()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Ok(new
            {
                userId = userId,
                username = username,
                role = role,
                message = "Acceso concedido a usuario autenticado"
            });
        }

        [HttpGet]
        [Route("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminOnly()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            return Ok(new
            {
                username = username,
                message = "Acceso exclusivo para administradores"
            });
        }

        [HttpGet]
        [Route("public")]
        [AllowAnonymous]
        public IActionResult PublicEndpoint()
        {
            return Ok(new
            {
                message = "Este es un endpoint público"
            });
        }
    }
}
