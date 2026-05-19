using Microsoft.AspNetCore.Mvc;
using ParcialFJCO.Application.Interface;
using ParcialFJCO.Domain.DTO;

namespace ParcialFJCO.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUserN request)
        {
            var result = await _loginService.Login(request);
            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }
    }
}
