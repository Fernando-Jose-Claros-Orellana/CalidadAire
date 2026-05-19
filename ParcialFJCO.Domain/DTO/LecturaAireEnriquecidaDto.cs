namespace ParcialFJCO.Domain.DTO
{
    public class LecturaAireEnriquecidaDto : LecturaAireDto
    {
        public double Temperatura { get; set; }
        public int Humedad { get; set; }
        public string DescripcionClima { get; set; } = string.Empty;
    }
}
