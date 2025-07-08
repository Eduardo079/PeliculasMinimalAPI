using PeliculasMinimalAPI.DTOs;
using PeliculasMinimalAPI.Entidades;

namespace PeliculasMinimalAPI.Repositorio
{
    public interface IRepositorioPeliculas
    {
        Task Actualizar(Pelicula pelicula);
        Task Borrar(int Id);
        Task<int> Crear(Pelicula pelicula);
        Task<bool> Existe(int Id);
        Task<Pelicula?> ObtenerPorId(int Id);
        Task<List<Pelicula>> ObtenerPorNombre(string titulo);
        Task<List<Pelicula>> ObtenerTodos(PaginacionDTOs paginacionDTOs);
    }
}