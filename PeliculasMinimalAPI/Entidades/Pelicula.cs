namespace PeliculasMinimalAPI.Entidades
{
    public class Pelicula
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string EnCines { get; set; }
        public DateTime FechaLanzamineto { get; set; }
        public  string? Poster { get; set; }
        public List<Comentario> Comentarios { get; set; } = new List<Comentario>();
    }
}
