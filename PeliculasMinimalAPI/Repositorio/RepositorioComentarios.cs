﻿using Microsoft.EntityFrameworkCore;
using PeliculasMinimalAPI.Entidades;

namespace PeliculasMinimalAPI.Repositorio
{
    public class RepositorioComentarios : IRepositorioComentarios
    {
        private readonly ApplicationDBContext context;

        public RepositorioComentarios(ApplicationDBContext context)
        {
            this.context = context;
        }

        public async Task<List<Comentario>> ObtenerListado(int PeliculaId)
        {
            return await context.Comentarios.Where(c => c.PeliculaId == PeliculaId).ToListAsync();

        }

        public async Task<Comentario?> ObtenerPorId(int id)
        {
            return await context.Comentarios.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<int> Crear(Comentario comentario)
        {
            context.Add(comentario);
            await context.SaveChangesAsync();
            return comentario.Id;
        }

        public async Task<bool> Existe(int id)
        {
            return await context.Comentarios.AnyAsync(c => c.Id == id);
        }

        public async Task Actualizar(Comentario comentario)
        {
            context.Update(comentario);
            await context.SaveChangesAsync();

        }

        public async Task Borrar(int id)
        {
            await context.Comentarios.Where(c => c.Id == id).ExecuteDeleteAsync();
        }
    }
}
