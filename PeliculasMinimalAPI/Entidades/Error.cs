namespace PeliculasMinimalAPI.Entidades
{
    public class Error
    {
        public Guid Id { get; set; }
        public string MessageDeError { get; set; } = null!;
        public string StackTrace { get; set; }
        public  DateTime Fecha { get; set; }
    }
}
