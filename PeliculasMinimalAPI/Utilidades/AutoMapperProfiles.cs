using AutoMapper;
using PeliculasMinimalAPI.DTOs;
using PeliculasMinimalAPI.Entidades;
using PeliculasMinimalAPI.Migrations;

namespace PeliculasMinimalAPI.Utilidades
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CrearGeneroDTOs, Genero>();
            CreateMap<Genero, GeneroDTOs>();

            CreateMap<CrearActorDTOs, Actor>()
                .ForMember(x => x.Foto, opciones => opciones.Ignore());
            CreateMap<Actor, ActorDTOs>();

            CreateMap<CrearPeliculasDTOs, Pelicula>()
                .ForMember(x => x.Poster, opciones => opciones.Ignore());
            CreateMap<Pelicula, PeliculaDTOs>()
                .ForMember(p => p.Genero, Entidad => Entidad.MapFrom(p => p.GeneroPeliculas.Select(gp => new GeneroDTOs { Id = gp.GeneroId, Nombre = gp.Genero.Nombre })))
                .ForMember(x => x.Actores, Entidad => Entidad.MapFrom(a => a.ActorPeliculas.Select(ap => new ActorPeliculaDTOs { Id = ap.ActorId, Nombre = ap.Actor.Nombre, Personaje = ap.Personaje })));

            CreateMap<CrearComentariosDTOs, Comentario>();
            CreateMap<Comentario, ComentariosDTOs>();

            CreateMap<AsignarActorPeliculaDTOs, ActorPelicula>();
        }
    }
}
