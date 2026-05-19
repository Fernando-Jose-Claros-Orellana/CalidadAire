using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParcialFJCO.Application.Interface;
using ParcialFJCO.Domain.DTO;

namespace ParcialFJCO.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LecturasController : ControllerBase
    {
        private readonly ILecturasService _lecturasService;

        public LecturasController(ILecturasService lecturasService)
        {
            _lecturasService = lecturasService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerLecturas(
            [FromQuery] DateTime? fechaDesde,
            [FromQuery] DateTime? fechaHasta,
            [FromQuery] int? sensorId,
            [FromQuery] string? contaminante)
        {
            var lecturas = await _lecturasService.ObtenerLecturasAsync(
                fechaDesde, fechaHasta, sensorId, contaminante);
            return Ok(lecturas);
        }

        [HttpGet("enriquecidas")]
        public async Task<IActionResult> ObtenerLecturasEnriquecidas(
            [FromQuery] DateTime? fechaDesde,
            [FromQuery] DateTime? fechaHasta,
            [FromQuery] int? sensorId,
            [FromQuery] string? contaminante)
        {
            var lecturas = await _lecturasService.ObtenerLecturasEnriquecidasAsync(
                fechaDesde, fechaHasta, sensorId, contaminante);
            return Ok(lecturas);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarLectura([FromBody] RegistrarLecturaRequest request)
        {
            try
            {
                var response = await _lecturasService.RegistrarLecturaAsync(request);
                return CreatedAtAction(nameof(RegistrarLectura), new { id = response.Lectura.Id }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
