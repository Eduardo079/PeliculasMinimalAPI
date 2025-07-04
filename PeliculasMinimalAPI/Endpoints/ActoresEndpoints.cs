using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using PeliculasMinimalAPI.DTOs;
using PeliculasMinimalAPI.Entidades;
using PeliculasMinimalAPI.Repositorio;
using PeliculasMinimalAPI.Servicios;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace PeliculasMinimalAPI.Endpoints
{
    public static class ActoresEndpoints
    {
        private static readonly string contenedor = "actores";
        public static RouteGroupBuilder MapActores(this RouteGroupBuilder group)
        {
            group.MapGet("/", ObtenerActores).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("actores-get"));
            group.MapGet("/{id:int}", ObtenerActoresById);
            group.MapGet("obtenerPorNombre/{nombre}", ObtenerActoresPorNombre);
            group.MapPost("/", Crear).DisableAntiforgery();
            group.MapPut("/{id:int}", Actualizar).DisableAntiforgery();
            group.MapDelete("/{id:int}", Borrar);

            return group;
        }


        static async Task<Ok<List<ActorDTOs>>> ObtenerActores (IRepositorioActores repositorio, IMapper mapper, int pagina=1, int recordsPorPagina= 10)
        {
            var paginacion = new PaginacionDTOs { Pagina = pagina, RecordsPorPagina = recordsPorPagina };
            var actores = await repositorio.ObtenerTodos(paginacion);
            var actorDTOS = mapper.Map<List<ActorDTOs>>(actores);
            return TypedResults.Ok(actorDTOS);
        }
        static async Task<Ok<List<ActorDTOs>>> ObtenerActoresPorNombre(string nombre,IRepositorioActores repositorio, IMapper mapper)
        {
            var actores = await repositorio.ObtenerPorNombre(nombre);
            var actorDTOS = mapper.Map<List<ActorDTOs>>(actores);
            return TypedResults.Ok(actorDTOS);
        }

        static async Task<Results<Ok<ActorDTOs>, NotFound>> ObtenerActoresById(int id, IRepositorioActores repositorio, IMapper mapper)
        {
            var actor = await repositorio.ObtenerPorId(id);
            if (actor == null)
            {
                return TypedResults.NotFound();
            }
            var actorDTOs = mapper.Map<ActorDTOs>(actor);
            return TypedResults.Ok(actorDTOs);
        }

        static async Task<Created<ActorDTOs>> Crear([FromForm] CrearActorDTOs crearActorDTOs, IRepositorioActores repositorio, IOutputCacheStore outputCacheStore, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            var actor = mapper.Map<Actor>(crearActorDTOs);

            if (crearActorDTOs.Foto is not null)
            {
                var url = await almacenadorArchivos.Almacenar(contenedor, crearActorDTOs.Foto);
                actor.Foto = url;
            }

            var id = await repositorio.Crear(actor);
            await outputCacheStore.EvictByTagAsync("actores-get", default);
            var actorDTOs = mapper.Map<ActorDTOs>(actor);
            return TypedResults.Created($"/actores/{id}", actorDTOs);
        }

        static async Task<Results<NoContent, NotFound>> Actualizar(int id,
            [FromForm] CrearActorDTOs crearActorDTOs, IRepositorioActores repositorio,
            IAlmacenadorArchivos almacenadorArchivos, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var actorDB = await repositorio.ObtenerPorId(id);

            if (actorDB is null)
            {
                return TypedResults.NotFound();
            }

            var actorParaActualizar = mapper.Map<Actor>(crearActorDTOs);
            actorParaActualizar.Id = id;
            actorParaActualizar.Foto = actorDB.Foto;

            if (crearActorDTOs.Foto is not null)
            {
                var url = await almacenadorArchivos.Editar(actorParaActualizar.Foto,
                    contenedor, crearActorDTOs.Foto);
                actorParaActualizar.Foto = url;
            }

            await repositorio.Actualizar(actorParaActualizar);
            await outputCacheStore.EvictByTagAsync("actores-get", default);
            return TypedResults.NoContent();
        }



        static async Task<Results<NoContent, NotFound>> Borrar(int id, IRepositorioActores repositorioActores, IOutputCacheStore outputCacheStore)
        {
            var existe = await repositorioActores.Existe(id);
            if (!existe)
            {
                return TypedResults.NotFound();
            }
            await repositorioActores.Borrar(id);
            await outputCacheStore.EvictByTagAsync("actores-get", default);
            return TypedResults.NoContent();
        }
    }
}
