namespace ParcialFJCO.Domain.Entities
{
    public class LecturaAire
    {
        public int Id { get; set; }
        public int SensorId { get; set; }
        public decimal PM2_5 { get; set; }
        public decimal PM10 { get; set; }
        public decimal CO2 { get; set; }
        public DateTime FechaHora { get; set; }

        public SensorCalidadAire? Sensor { get; set; }
    }
}
