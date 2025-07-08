using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
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
            group.MapPost("/", Crear).DisableAntiforgery();
            return group;
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
    }
}
