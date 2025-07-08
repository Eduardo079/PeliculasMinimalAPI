namespace PeliculasMinimalAPI.DTOs
{
    public class PeliculaDTOs
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string EnCines { get; set; }
        public DateTime FechaLanzamineto { get; set; }
        public string? Poster { get; set; }
    }
}
