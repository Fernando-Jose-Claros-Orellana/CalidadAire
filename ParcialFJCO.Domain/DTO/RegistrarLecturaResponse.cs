namespace ParcialFJCO.Domain.DTO
{
    public class RegistrarLecturaResponse
    {
        public LecturaAireDto Lectura { get; set; } = null!;
        public AlertaAireDto? Alerta { get; set; }
    }
}
