using PeliculasMinimalAPI.Entidades;

namespace PeliculasMinimalAPI.DTOs
{
    public class PeliculaDTOs
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string EnCines { get; set; }
        public DateTime FechaLanzamineto { get; set; }
        public string? Poster { get; set; }

        public List<Comentario> Comentarios { get; set; } = new List<Comentario>();

        public List<GeneroDTOs> Genero { get; set; } = new List<GeneroDTOs>();
        public List<ActorPeliculaDTOs> Actores { get; set; } = new List<ActorPeliculaDTOs>();
    }
}
