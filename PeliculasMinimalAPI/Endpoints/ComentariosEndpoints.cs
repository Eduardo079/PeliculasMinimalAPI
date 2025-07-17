using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using PeliculasMinimalAPI.DTOs;
using PeliculasMinimalAPI.Entidades;
using PeliculasMinimalAPI.Filtros;
using PeliculasMinimalAPI.Repositorio;

namespace PeliculasMinimalAPI.Endpoints
{
    public static class ComentariosEndpoints
    {
        public static RouteGroupBuilder MapComentarios(this RouteGroupBuilder group)
        {
            group.MapGet("/", ObtenerTodo).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("comentario-get"));
            group.MapGet("/{id:int}", ObtenerPorId).WithName("ObtenerComentarioPorId");
            group.MapPost("/", Crear).AddEndpointFilter<FiltroValidaciones<CrearComentariosDTOs>>();
            group.MapPut("/{id:int}",Actualizar).AddEndpointFilter<FiltroValidaciones<CrearComentariosDTOs>>();
            group.MapDelete("/{id:int}", Borrar);
            return group;
        }

        static async Task<Results<Ok<List<ComentariosDTOs>>, NoContent>> ObtenerTodo(int PeliculaId, IRepositorioComentarios repositorioComentarios, IRepositorioPeliculas repositorioPeliculas, IMapper mapper)
        {
            if (!await repositorioPeliculas.Existe(PeliculaId))
            {
                return TypedResults.NoContent();
            }
            var comentario = await repositorioComentarios.ObtenerListado(PeliculaId);
            var comentarioDTO = mapper.Map<List<ComentariosDTOs>>(comentario);
            return TypedResults.Ok(comentarioDTO);

        }

        static async Task<Results<Ok<ComentariosDTOs>, NoContent>> ObtenerPorId(int peliculaId,int id,IRepositorioComentarios repositorioComentarios, IRepositorioPeliculas repositorioPeliculas, IMapper mapper)
        {
            var comentario = await repositorioComentarios.ObtenerPorId(id);

            if(comentario is null)
            {
                return TypedResults.NoContent();
            }

            var comentarioDTO = mapper.Map<ComentariosDTOs>(comentario);
            return TypedResults.Ok(comentarioDTO);
        }

        static async Task<Results<CreatedAtRoute<ComentariosDTOs>, NotFound>> Crear(int PeliculaId, CrearComentariosDTOs crearComentariosDTOs,
            IRepositorioComentarios repositorioComentarios,
            IRepositorioPeliculas repositorioPeliculas,IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            if (! await repositorioPeliculas.Existe(PeliculaId))
            {
                return TypedResults.NotFound();
            }

            var comentario = mapper.Map<Comentario>(crearComentariosDTOs);
            comentario.PeliculaId = PeliculaId;
            var id = await repositorioComentarios.Crear(comentario);
            await outputCacheStore.EvictByTagAsync("comentarios-get", default);
            var comentariosDTO = mapper.Map<ComentariosDTOs>(comentario);
            return TypedResults.CreatedAtRoute(comentariosDTO, "ObtenerComentarioPorId", new { id, PeliculaId });
        }


        static async Task<Results<NoContent, NotFound>> Actualizar(int peliculaId, int id, CrearComentariosDTOs crearComentariosDTOs,
            IOutputCacheStore outputCacheStore, IRepositorioComentarios repositorioComentarios, IRepositorioPeliculas repositorioPeliculas, IMapper mapper)
        {
            if (!await repositorioPeliculas.Existe(peliculaId)) { return TypedResults.NotFound(); }

            if (!await repositorioComentarios.Existe(id)) { return TypedResults.NotFound(); };

            var comentario = mapper.Map<Comentario>(crearComentariosDTOs);
            comentario.Id = id;
            comentario.PeliculaId = peliculaId;

            await repositorioComentarios.Actualizar(comentario);
            await outputCacheStore.EvictByTagAsync("comentarios-get", default);
            return TypedResults.NoContent();

        }


        static async Task<Results<NoContent, NotFound>> Borrar(int peliculaId,int id, IRepositorioComentarios repositorio,IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            if (!await repositorio.Existe(id)) { return TypedResults.NoContent(); }
            

            await repositorio.Borrar(id);
            await outputCacheStore.EvictByTagAsync("comentario-get", default);
            return TypedResults.NoContent();
        }
        
    }
}
