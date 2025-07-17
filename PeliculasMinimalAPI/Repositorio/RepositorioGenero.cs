using Microsoft.EntityFrameworkCore;
using PeliculasMinimalAPI.Entidades;

namespace PeliculasMinimalAPI.Repositorio
{
    public class RepositorioGenero : IRepositorioGeneros
    {
        private readonly ApplicationDBContext context;
        public RepositorioGenero(ApplicationDBContext context)
        {
            this.context = context;
        }

        

        public async Task<int> Crear(Genero genero)
        {

            context.Add(genero);
            await context.SaveChangesAsync();
            return genero.Id;
        }

        public async Task Actualizar(Genero genero)
        {
            context.Update(genero);
            await context.SaveChangesAsync();
        }

        public async Task<bool> Existe(int id)
        {
            return await context.Generos.AnyAsync(o => o.Id == id);
        }

        public async Task<bool> Existe(int id, string nombre)
        {
            return await context.Generos.AnyAsync(g => g.Id != id && g.Nombre == nombre);
        }

        public async Task<List<int>> Existen(List<int> ids)
        {
            return await context.Generos.Where(c =>  ids.Contains(c.Id)).Select(c => c.Id).ToListAsync();
        }

        public async Task<Genero?> ObtenerPorId(int id)
        {
            return await context.Generos.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Genero>> ObtenerTodos()
        {
            return await context.Generos.OrderByDescending(x => x.Nombre).ToListAsync();
        }

        public async Task Borrar(int id)
        {
            await context.Generos.Where(x => x.Id == id).ExecuteDeleteAsync();
        }
    }
}
