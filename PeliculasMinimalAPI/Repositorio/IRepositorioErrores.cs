using PeliculasMinimalAPI.Entidades;

namespace PeliculasMinimalAPI.Repositorio
{
    public interface IRepositorioErrores
    {
        Task Crear(Error error);
    }
}