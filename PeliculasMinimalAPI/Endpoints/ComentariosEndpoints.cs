using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using PeliculasMinimalAPI.DTOs;
using PeliculasMinimalAPI.Entidades;
using PeliculasMinimalAPI.Repositorio;

namespace PeliculasMinimalAPI.Endpoints
{
    public static class ComentariosEndpoints
    {
        public static RouteGroupBuilder MapComentarios(this RouteGroupBuilder group)
        {
            group.MapPost("/", Crear);
            return group;
        }

        static async Task<Results<Created<ComentariosDTOs>, NotFound>> Crear(int PeliculaId, CrearComentariosDTOs crearComentariosDTOs,
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
            return TypedResults.Created($"/comentario/{id}", comentariosDTO);
        }

        

    }
}
