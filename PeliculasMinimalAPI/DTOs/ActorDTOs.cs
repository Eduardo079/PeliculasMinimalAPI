﻿namespace PeliculasMinimalAPI.DTOs
{
    public class ActorDTOs
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }
        public string? Foto { get; set; }
    }
}
