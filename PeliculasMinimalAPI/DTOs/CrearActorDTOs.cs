namespace PeliculasMinimalAPI.DTOs
{
    public class CrearActorDTOs
    {
        
        public string Nombre { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }
        public IFormFile? Foto { get; set; }
    }
}
