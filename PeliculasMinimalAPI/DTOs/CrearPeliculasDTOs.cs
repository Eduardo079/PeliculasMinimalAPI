namespace PeliculasMinimalAPI.DTOs
{
    public class CrearPeliculasDTOs
    {
        public string Titulo { get; set; } = null!;
        public string EnCines { get; set; }
        public DateTime FechaLanzamineto { get; set; }
        public IFormFile? Poster { get; set; }
    }
}
