using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore.Query;
using PeliculasMinimalAPI.DTOs;
using PeliculasMinimalAPI.Entidades;
using PeliculasMinimalAPI.Repositorio;
using PeliculasMinimalAPI.Servicios;

namespace PeliculasMinimalAPI.Endpoints
{
    public static class PeliculaEndpoint
    {
        private static readonly string contenedor = "pelicula";
        public static RouteGroupBuilder MapPeliculas(this RouteGroupBuilder group)
        {
            group.MapGet("/", Obtener).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("peliculas-get"));
            group.MapGet("/{id:int}", ObtenerPorId).DisableAntiforgery();
            group.MapPost("/", Crear).DisableAntiforgery();
            group.MapPut("/{id:int}",Actualizar).DisableAntiforgery();
            group.MapDelete("/{id:int}", Borrar);
            return group;
        }

        static async Task<Ok<List<PeliculaDTOs>>> Obtener(IRepositorioPeliculas repositorio, IMapper mapper, int pagina = 1, int recordspaginacion = 10)
        {
            var paginacion = new PaginacionDTOs { Pagina = pagina, RecordsPorPagina = recordspaginacion };
            var peliculas = await repositorio.ObtenerTodos(paginacion);
            var peliculasDTOs = mapper.Map<List<PeliculaDTOs>>(peliculas);
            return TypedResults.Ok(peliculasDTOs);
        }

        static async Task<Results<Ok<PeliculaDTOs>, NotFound>> ObtenerPorId(int id, IRepositorioPeliculas repositorio, IMapper mapper)
        {
            var pelicula = await repositorio.ObtenerPorId(id);
            if (pelicula == null)
            {
                return TypedResults.NotFound();
            }
            var peliculasDTOs = mapper.Map<PeliculaDTOs>(pelicula);
            return TypedResults.Ok(peliculasDTOs);

        }
        static async Task<Created<PeliculaDTOs>> Crear([FromForm] CrearPeliculasDTOs crearPeliculasDTOs, IRepositorioPeliculas repositorio,
            IAlmacenadorArchivos almacenadorArchivos, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var pelicula = mapper.Map<Pelicula>(crearPeliculasDTOs);

            if (crearPeliculasDTOs.Poster is not null)
            {
                var url = await almacenadorArchivos.Almacenar(contenedor, crearPeliculasDTOs.Poster);
                pelicula.Poster = url;
            }

            var id = await repositorio.Crear(pelicula);
            await outputCacheStore.EvictByTagAsync("peliculas-get", default);
            var peliculaDTO = mapper.Map<PeliculaDTOs>(pelicula);
            return TypedResults.Created($"/pelicula/{id}", peliculaDTO);
        }


        static async Task<Results<NoContent, NotFound>> Actualizar(int id, [FromForm] CrearPeliculasDTOs crearPeliculasDTOs,
            IRepositorioPeliculas repositorio, IAlmacenadorArchivos almacenadorArchivos, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var DBPeliculas = await repositorio.ObtenerPorId(id);
            if (DBPeliculas == null)
            {
                return TypedResults.NoContent();
            }

            var actualizarPelicula = mapper.Map<Pelicula>(crearPeliculasDTOs);
            actualizarPelicula.Id = id;
            actualizarPelicula.Poster = DBPeliculas.Poster;

            if (crearPeliculasDTOs.Poster is not null)
            {
                var url = await almacenadorArchivos.Editar(actualizarPelicula.Poster, contenedor, crearPeliculasDTOs.Poster);
                actualizarPelicula.Poster = url;
            }

            await repositorio.Actualizar(actualizarPelicula);
            await outputCacheStore.EvictByTagAsync("peliculas-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> Borrar(int id, IRepositorioPeliculas repositorio, IAlmacenadorArchivos almacenadorArchivos, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var eliminarPelicula = await repositorio.ObtenerPorId(id);
            if(eliminarPelicula is null)
            {
                return TypedResults.NoContent();
            }

            await repositorio.Borrar(id);
            await almacenadorArchivos.Borrar(eliminarPelicula.Poster, contenedor);
            await outputCacheStore.EvictByTagAsync("peliculas-get", default);
            return TypedResults.NoContent();
        }
    }
}
