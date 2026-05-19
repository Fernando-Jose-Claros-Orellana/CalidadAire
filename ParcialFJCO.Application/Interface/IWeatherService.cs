using ParcialFJCO.Domain.DTO;

namespace ParcialFJCO.Application.Interface
{
    public interface IWeatherService
    {
        Task<WeatherInfo?> ObtenerClimaActualAsync();
    }
}
