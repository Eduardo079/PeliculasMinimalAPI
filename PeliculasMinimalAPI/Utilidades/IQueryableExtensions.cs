using PeliculasMinimalAPI.DTOs;

namespace PeliculasMinimalAPI.Utilidades
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, PaginacionDTOs paginacionDTOs)
        {
            return queryable.Skip((paginacionDTOs.Pagina - 1) * paginacionDTOs.RecordsPorPagina).Take(paginacionDTOs.RecordsPorPagina);
        }
    }
}
