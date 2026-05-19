using ParcialFJCO.Domain.DTO;

namespace ParcialFJCO.Application.Interface
{
    public interface ILecturasService
    {
        Task<RegistrarLecturaResponse> RegistrarLecturaAsync(RegistrarLecturaRequest request);
        Task<IEnumerable<LecturaAireDto>> ObtenerLecturasAsync(
            DateTime? fechaDesde,
            DateTime? fechaHasta,
            int? sensorId,
            string? contaminante);
        Task<IEnumerable<LecturaAireEnriquecidaDto>> ObtenerLecturasEnriquecidasAsync(
            DateTime? fechaDesde,
            DateTime? fechaHasta,
            int? sensorId,
            string? contaminante);
    }
}
