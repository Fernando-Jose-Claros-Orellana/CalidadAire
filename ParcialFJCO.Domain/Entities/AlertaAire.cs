namespace ParcialFJCO.Domain.Entities
{
    public class AlertaAire
    {
        public int Id { get; set; }
        public int SensorId { get; set; }
        public string Nivel { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }

        public SensorCalidadAire? Sensor { get; set; }
    }
}
