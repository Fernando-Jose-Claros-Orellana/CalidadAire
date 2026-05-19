using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParcialFJCO.Application.Interface;

namespace ParcialFJCO.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AlertasController : ControllerBase
    {
        private readonly IAlertasService _alertasService;

        public AlertasController(IAlertasService alertasService)
        {
            _alertasService = alertasService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerAlertas()
        {
            var alertas = await _alertasService.ObtenerAlertasAsync();
            return Ok(alertas);
        }
    }
}
