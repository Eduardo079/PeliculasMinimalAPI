using Microsoft.EntityFrameworkCore;
using PeliculasMinimalAPI.DTOs;
using PeliculasMinimalAPI.Entidades;
using PeliculasMinimalAPI.Migrations;
using PeliculasMinimalAPI.Utilidades;

namespace PeliculasMinimalAPI.Repositorio
{
    public class RepositorioPeliculas : IRepositorioPeliculas
    {
        private readonly ApplicationDBContext context;
        private readonly HttpContext httpContext;

        public RepositorioPeliculas(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            httpContext = httpContextAccessor.HttpContext!;
        }

        public async Task<List<Pelicula>> ObtenerTodos(PaginacionDTOs paginacionDTOs)
        {
            var queryable = context.Peliculas.AsQueryable();
            await httpContext.InsetarParametrosPaginacionEncabecera(queryable);
            return await queryable.OrderBy(p => p.Titulo).Paginar(paginacionDTOs).ToListAsync();
        }

        public async Task<Pelicula?> ObtenerPorId(int Id)
        {
            return await context.Peliculas.Include(p => p.Comentarios).AsNoTracking().FirstOrDefaultAsync(p => p.Id == Id);
        }

        public async Task<List<Pelicula>> ObtenerPorNombre(string titulo)
        {
            return await context.Peliculas.Where(p => p.Titulo.Contains(titulo)).OrderBy(p => p.Titulo == titulo).ToListAsync();
        }

        public async Task<bool> Existe(int Id)
        {
            return await context.Peliculas.AnyAsync(p => p.Id == Id);
        }
        public async Task<int> Crear(Pelicula pelicula)
        {
            context.Add(pelicula);
            await context.SaveChangesAsync();
            return pelicula.Id;
        }

        public async Task Actualizar(Pelicula pelicula)
        {
            context.Update(pelicula);
            await context.SaveChangesAsync();
        }

        public async Task Borrar(int Id)
        {
            await context.Peliculas.Where(p => p.Id == Id).ExecuteDeleteAsync();
        }
    }
}
