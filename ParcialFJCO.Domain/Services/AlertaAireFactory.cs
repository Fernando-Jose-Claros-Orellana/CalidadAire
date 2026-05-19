using ParcialFJCO.Domain.Entities;

namespace ParcialFJCO.Domain.Services
{
    public static class AlertaAireFactory
    {
        public static AlertaAire? CrearDesdeLectura(LecturaAire lectura)
        {
            if (lectura.CO2 > 5000 || lectura.PM2_5 > 250)
            {
                return new AlertaAire
                {
                    SensorId = lectura.SensorId,
                    Nivel = "Extrema",
                    Mensaje = "Nivel de contaminación extremadamente alto. Riesgo severo para la salud.",
                    FechaHora = lectura.FechaHora
                };
            }

            if (lectura.PM2_5 > 150 || lectura.PM10 > 200)
            {
                return new AlertaAire
                {
                    SensorId = lectura.SensorId,
                    Nivel = "Critica",
                    Mensaje = "La calidad del aire es peligrosa. Se recomienda permanecer en interiores y usar mascarilla.",
                    FechaHora = lectura.FechaHora
                };
            }

            if ((lectura.PM2_5 >= 51 && lectura.PM2_5 <= 100) || lectura.CO2 > 1000)
            {
                return new AlertaAire
                {
                    SensorId = lectura.SensorId,
                    Nivel = "Moderada",
                    Mensaje = "La calidad del aire es poco saludable para grupos sensibles (niños, adultos mayores, personas con enfermedades respiratorias).",
                    FechaHora = lectura.FechaHora
                };
            }

            if (lectura.PM2_5 >= 25 && lectura.PM2_5 <= 50)
            {
                return new AlertaAire
                {
                    SensorId = lectura.SensorId,
                    Nivel = "Leve",
                    Mensaje = "La calidad del aire es moderada, se recomienda reducir actividades al aire libre prolongadas.",
                    FechaHora = lectura.FechaHora
                };
            }

            return null;
        }
    }
}
