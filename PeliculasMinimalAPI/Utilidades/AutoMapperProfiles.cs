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
        }
    }
}
