using PeliculasMinimalAPI.Entidades;

namespace PeliculasMinimalAPI.Repositorio
{
    public interface IRepositorioComentarios
    {
        Task Actualizar(Comentario comentario);
        Task Borrar(int id);
        Task<int> Crear(Comentario comentario);
        Task<bool> Existe(int id);
        Task<List<Comentario>> ObtenerListado(int PeliculaId);
        Task<Comentario?> ObtenerPorId(int id);
    }
}