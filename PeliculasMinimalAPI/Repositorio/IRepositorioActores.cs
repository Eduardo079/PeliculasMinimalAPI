using PeliculasMinimalAPI.DTOs;
using PeliculasMinimalAPI.Utilidades;

namespace PeliculasMinimalAPI.Repositorio
{
    public interface IRepositorioActores
    {
        Task Actualizar(Actor actor);
        Task Borrar(int id);
        Task<int> Crear(Actor actor);
        Task<bool> Existe(int id);
        Task<Actor?> ObtenerPorId(int id);
        Task<List<Actor>> ObtenerPorNombre(string nombre);
        Task<List<Actor>> ObtenerTodos(PaginacionDTOs paginacionDTOs);
    }
}