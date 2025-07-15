namespace PeliculasMinimalAPI.Entidades
{
    public class ComentariosDTOs
    {
        public int Id { get; set; }
        public string Cuerpo { get; set; } = null!;
        public int PeliculaId { get; set; }
    }
}
