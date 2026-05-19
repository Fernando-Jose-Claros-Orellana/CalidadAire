using Microsoft.Extensions.Configuration;
using ParcialFJCO.Application.Interface;
using ParcialFJCO.Domain.DTO;
using System.Text.Json;

namespace ParcialFJCO.Infraestructure.Service
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<WeatherInfo?> ObtenerClimaActualAsync()
        {
            try
            {
                var apiKey = _configuration["OpenWeather:ApiKey"];
                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    return null;
                }

                var url = $"https://api.openweathermap.org/data/2.5/weather?q=San Salvador,SV&units=metric&appid={apiKey}";
                using var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                using var stream = await response.Content.ReadAsStreamAsync();
                using var json = await JsonDocument.ParseAsync(stream);

                var root = json.RootElement;
                var main = root.GetProperty("main");
                var weatherArray = root.GetProperty("weather");
                var firstWeather = weatherArray.GetArrayLength() > 0 ? weatherArray[0] : default;

                return new WeatherInfo
                {
                    Temperatura = main.GetProperty("temp").GetDouble(),
                    Humedad = main.GetProperty("humidity").GetInt32(),
                    Descripcion = firstWeather.ValueKind == JsonValueKind.Object
                        ? firstWeather.GetProperty("description").GetString() ?? string.Empty
                        : string.Empty
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
