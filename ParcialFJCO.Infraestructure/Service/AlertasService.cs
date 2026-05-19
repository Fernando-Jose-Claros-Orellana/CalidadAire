using Microsoft.EntityFrameworkCore;
using ParcialFJCO.Application.Interface;
using ParcialFJCO.Domain.DTO;
using ParcialFJCO.Infraestructure.Data;

namespace ParcialFJCO.Infraestructure.Service
{
    public class AlertasService : IAlertasService
    {
        private readonly AppDbContext _db;

        public AlertasService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<AlertaAireDto>> ObtenerAlertasAsync()
        {
            return await _db.AlertasAire
                .OrderByDescending(a => a.FechaHora)
                .Select(a => new AlertaAireDto
                {
                    Id = a.Id,
                    SensorId = a.SensorId,
                    Nivel = a.Nivel,
                    Mensaje = a.Mensaje,
                    FechaHora = a.FechaHora
                })
                .ToListAsync();
        }
    }
}
