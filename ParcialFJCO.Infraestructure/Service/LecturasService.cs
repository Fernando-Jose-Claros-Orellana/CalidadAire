using Microsoft.EntityFrameworkCore;
using ParcialFJCO.Application.Interface;
using ParcialFJCO.Domain.DTO;
using ParcialFJCO.Domain.Entities;
using ParcialFJCO.Domain.Services;
using ParcialFJCO.Infraestructure.Data;

namespace ParcialFJCO.Infraestructure.Service
{
    public class LecturasService : ILecturasService
    {
        private readonly AppDbContext _db;
        private readonly IWeatherService _weatherService;

        public LecturasService(AppDbContext db, IWeatherService weatherService)
        {
            _db = db;
            _weatherService = weatherService;
        }

        public async Task<RegistrarLecturaResponse> RegistrarLecturaAsync(RegistrarLecturaRequest request)
        {
            var sensor = await _db.SensoresCalidadAire
                .FirstOrDefaultAsync(s => s.Id == request.SensorId);

            if (sensor == null)
            {
                throw new KeyNotFoundException("Sensor no encontrado.");
            }

            if (request.PM2_5 < 0 || request.PM10 < 0 || request.CO2 < 0)
            {
                throw new ArgumentException("Los valores de la lectura no pueden ser negativos.");
            }

            var lectura = new LecturaAire
            {
                SensorId = request.SensorId,
                PM2_5 = request.PM2_5,
                PM10 = request.PM10,
                CO2 = request.CO2,
                FechaHora = DateTime.UtcNow
            };

            _db.LecturasAire.Add(lectura);
            await _db.SaveChangesAsync();

            var alerta = AlertaAireFactory.CrearDesdeLectura(lectura);
            if (alerta != null)
            {
                _db.AlertasAire.Add(alerta);
                await _db.SaveChangesAsync();
            }

            return new RegistrarLecturaResponse
            {
                Lectura = new LecturaAireDto
                {
                    Id = lectura.Id,
                    SensorId = lectura.SensorId,
                    PM2_5 = lectura.PM2_5,
                    PM10 = lectura.PM10,
                    CO2 = lectura.CO2,
                    FechaHora = lectura.FechaHora
                },
                Alerta = alerta == null
                    ? null
                    : new AlertaAireDto
                    {
                        Id = alerta.Id,
                        SensorId = alerta.SensorId,
                        Nivel = alerta.Nivel,
                        Mensaje = alerta.Mensaje,
                        FechaHora = alerta.FechaHora
                    }
            };
        }

        public async Task<IEnumerable<LecturaAireDto>> ObtenerLecturasAsync(
            DateTime? fechaDesde,
            DateTime? fechaHasta,
            int? sensorId,
            string? contaminante)
        {
            var query = ConstruirConsultaLecturas(fechaDesde, fechaHasta, sensorId, contaminante);

            return await query
                .Select(l => new LecturaAireDto
                {
                    Id = l.Id,
                    SensorId = l.SensorId,
                    PM2_5 = l.PM2_5,
                    PM10 = l.PM10,
                    CO2 = l.CO2,
                    FechaHora = l.FechaHora
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<LecturaAireEnriquecidaDto>> ObtenerLecturasEnriquecidasAsync(
            DateTime? fechaDesde,
            DateTime? fechaHasta,
            int? sensorId,
            string? contaminante)
        {
            var query = ConstruirConsultaLecturas(fechaDesde, fechaHasta, sensorId, contaminante);
            var lecturas = await query
                .Select(l => new LecturaAireDto
                {
                    Id = l.Id,
                    SensorId = l.SensorId,
                    PM2_5 = l.PM2_5,
                    PM10 = l.PM10,
                    CO2 = l.CO2,
                    FechaHora = l.FechaHora
                })
                .ToListAsync();

            var weather = await _weatherService.ObtenerClimaActualAsync();
            var temperatura = weather?.Temperatura ?? 0;
            var humedad = weather?.Humedad ?? 0;
            var descripcion = weather?.Descripcion ?? string.Empty;

            return lecturas.Select(l => new LecturaAireEnriquecidaDto
            {
                Id = l.Id,
                SensorId = l.SensorId,
                PM2_5 = l.PM2_5,
                PM10 = l.PM10,
                CO2 = l.CO2,
                FechaHora = l.FechaHora,
                Temperatura = temperatura,
                Humedad = humedad,
                DescripcionClima = descripcion
            });
        }

        private IQueryable<LecturaAire> ConstruirConsultaLecturas(
            DateTime? fechaDesde,
            DateTime? fechaHasta,
            int? sensorId,
            string? contaminante)
        {
            var query = _db.LecturasAire.AsQueryable();

            if (sensorId.HasValue)
            {
                query = query.Where(l => l.SensorId == sensorId.Value);
            }

            if (fechaDesde.HasValue)
            {
                query = query.Where(l => l.FechaHora >= fechaDesde.Value);
            }

            if (fechaHasta.HasValue)
            {
                query = query.Where(l => l.FechaHora <= fechaHasta.Value);
            }

            if (!string.IsNullOrWhiteSpace(contaminante))
            {
                switch (contaminante.Trim().ToUpperInvariant())
                {
                    case "PM2_5":
                        query = query.OrderByDescending(l => l.PM2_5);
                        break;
                    case "PM10":
                        query = query.OrderByDescending(l => l.PM10);
                        break;
                    case "CO2":
                        query = query.OrderByDescending(l => l.CO2);
                        break;
                }
            }

            return query;
        }
    }
}
