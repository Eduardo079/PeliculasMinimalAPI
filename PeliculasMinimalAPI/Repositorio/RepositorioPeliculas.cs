using AutoMapper;
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
        private readonly IMapper mapper;
        private readonly HttpContext httpContext;

        public RepositorioPeliculas(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
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
            return await context.Peliculas.
                Include(p => p.Comentarios).
                Include(p => p.GeneroPeliculas).
                    ThenInclude(gp => gp.Genero).
                Include(p => p.ActorPeliculas.OrderBy(a => a.Orden)).
                    ThenInclude(gp => gp.Actor).
                AsNoTracking().FirstOrDefaultAsync(p => p.Id == Id);
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

        public async Task AsignarGeneros(int id, List<int> generosIds)
        {
            var pelicula = await context.Peliculas.Include(p => p.GeneroPeliculas).FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula is null)
            {
                throw new Exception($"No existe ninguan Pelicula con ese id{id}");
            }

            var generoPelicula = generosIds.Select(generoId => new GeneroPelicula() { GeneroId = generoId });

            pelicula.GeneroPeliculas = mapper.Map(generoPelicula, pelicula.GeneroPeliculas);
            await context.SaveChangesAsync();
        }

        public async Task AsignarActores (int id, List<ActorPelicula> actores)
        {
            for (int i = 1; i <= actores.Count; i++)
            {
                actores[i-1].Orden = i;

            }
            var pelicula = await context.Peliculas.Include(x => x.ActorPeliculas).FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula is null)
            {
                throw new Exception($"No existe la pelicula con el Id: {id}");
            }

            pelicula.ActorPeliculas = mapper.Map(actores , pelicula.ActorPeliculas);
            await context.SaveChangesAsync();


        }
    }
}
