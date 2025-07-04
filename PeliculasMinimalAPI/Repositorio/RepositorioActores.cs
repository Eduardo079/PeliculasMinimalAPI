using Microsoft.EntityFrameworkCore;
using PeliculasMinimalAPI.DTOs;
using PeliculasMinimalAPI.Entidades;
using PeliculasMinimalAPI.Utilidades;

namespace PeliculasMinimalAPI.Repositorio
{
    public class RepositorioActores : IRepositorioActores
    {
        private readonly ApplicationDBContext context;
        private readonly HttpContext httppContext;

        public RepositorioActores(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            httppContext = httpContextAccessor.HttpContext!;
        }

        public async Task<List<Actor>> ObtenerTodos(PaginacionDTOs paginacionDTOs)
        {
            var queryable = context.Actores.AsQueryable();
            await httppContext.InsetarParametrosPaginacionEncabecera(queryable);
            return await queryable.OrderBy(a => a.Nombre).Paginar(paginacionDTOs).ToListAsync();
        }

        public async Task<Actor?> ObtenerPorId(int id)
        {
            return await context.Actores.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Actor>> ObtenerPorNombre(string nombre)
        {
            return await context.Actores.Where(a => a.Nombre.Contains(nombre)).OrderBy(a => a.Nombre).ToListAsync();
        }
        public async Task<int> Crear(Actor actor)
        {
            context.Add(actor);
            await context.SaveChangesAsync();
            return actor.Id;

        }

        public async Task<bool> Existe(int id)
        {
            return await context.Actores.AnyAsync(a => a.Id == id);
        }


        public async Task Actualizar(Actor actor)
        {
            context.Update(actor);
            await context.SaveChangesAsync();

        }

        public async Task Borrar(int id)
        {
            await context.Actores.Where(a => a.Id == id).ExecuteDeleteAsync();
        }
    }
}
