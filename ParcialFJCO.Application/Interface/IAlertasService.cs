using ParcialFJCO.Domain.DTO;

namespace ParcialFJCO.Application.Interface
{
    public interface IAlertasService
    {
        Task<IEnumerable<AlertaAireDto>> ObtenerAlertasAsync();
    }
}
