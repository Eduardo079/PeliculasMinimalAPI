using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.IdentityModel.Tokens;
using PeliculasMinimalAPI.DTOs;
using PeliculasMinimalAPI.Entidades;
using PeliculasMinimalAPI.Filtros;
using PeliculasMinimalAPI.Migrations;
using PeliculasMinimalAPI.Repositorio;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PeliculasMinimalAPI.Endpoints
{
    public static class GeneroEndpoint
    {
        public static async Task<RouteGroupBuilder> MapGeneros(this RouteGroupBuilder group)
        {
            group.MapGet("/", ObtenerGeneros).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("generos-get"));
            group.MapGet("/{id:int}", ObtenerGeneroPorId);

            group.MapPost("/", CrearGenero).AddEndpointFilter<FiltroValidaciones<CrearGeneroDTOs>>();
            group.MapPut("/{id:int}", ActualizarGenero).AddEndpointFilter<FiltroValidaciones<CrearGeneroDTOs>>();
            group.MapDelete("/{id:int}", BorraGenero);

            return group;

        }

        static async Task<Ok<List<GeneroDTOs>>> ObtenerGeneros(IRepositorioGeneros repositorio, IMapper mapper)
        {
            var genero = await repositorio.ObtenerTodos();
            var  generoDTOs = mapper.Map<List<GeneroDTOs>>(genero);
            return TypedResults.Ok(generoDTOs);
        }

        static async Task<Results<Ok<GeneroDTOs>, NotFound>> ObtenerGeneroPorId(IRepositorioGeneros repositorio, int id, IMapper mapper)
        {
            var genero = await repositorio.ObtenerPorId(id);
            if (genero is null)
            {
                return TypedResults.NotFound();

            }
            var GeneroDTOs = mapper.Map<GeneroDTOs>(genero);
            return TypedResults.Ok(GeneroDTOs);
        }


        static async Task<Results<Created<GeneroDTOs>, ValidationProblem>> CrearGenero(CrearGeneroDTOs crearGeneroDTOs, IRepositorioGeneros repositorio,
            IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            
            var  genero = mapper.Map<Genero>(crearGeneroDTOs);
            var id = await repositorio.Crear(genero);
            await outputCacheStore.EvictByTagAsync("generos-get", default);
            var GeneroDTOs = mapper.Map<GeneroDTOs>(genero);
            return TypedResults.Created($"/generos/{id}", GeneroDTOs);
        }

        static async Task<Results<NoContent, NotFound, ValidationProblem>> ActualizarGenero(int id, CrearGeneroDTOs crearGeneroDTOs, 
            IRepositorioGeneros repositorio, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            

            var existe = await repositorio.Existe(id);

            if (!existe)
            {
                return TypedResults.NotFound();
            }
            var genero = mapper.Map<Genero>(crearGeneroDTOs);
            genero.Id = id;
            await repositorio.Actualizar(genero);
            await outputCacheStore.EvictByTagAsync("generos-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> BorraGenero(int id, IRepositorioGeneros repositorio, IOutputCacheStore outputCacheStore)
        {
            var existe = await repositorio.Existe(id);

            if (!existe)
            {
                return TypedResults.NotFound();
            }

            await repositorio.Borrar(id);
            await outputCacheStore.EvictByTagAsync("generos-get", default);
            return TypedResults.NoContent();
        }
    }
}
