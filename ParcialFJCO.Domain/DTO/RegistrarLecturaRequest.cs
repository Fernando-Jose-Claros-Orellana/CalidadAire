namespace ParcialFJCO.Domain.DTO
{
    public class RegistrarLecturaRequest
    {
        public int SensorId { get; set; }
        public decimal PM2_5 { get; set; }
        public decimal PM10 { get; set; }
        public decimal CO2 { get; set; }
    }
}
