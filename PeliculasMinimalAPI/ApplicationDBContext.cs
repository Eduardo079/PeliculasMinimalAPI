using Microsoft.EntityFrameworkCore;
using PeliculasMinimalAPI.Entidades;

namespace PeliculasMinimalAPI
{
    public class ApplicationDBContext : DbContext

    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Genero>().Property(P => P.Nombre).HasMaxLength(50);

            modelBuilder.Entity<Actor>().Property(P => P.Nombre).HasMaxLength(150);
            modelBuilder.Entity<Actor>().Property(P => P.Foto).IsUnicode(false);
            modelBuilder.Entity<Pelicula>().Property(P => P.Titulo).HasMaxLength(150);
            modelBuilder.Entity<Pelicula>().Property(P => P.Poster).IsUnicode(false);
        }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }

    }
}
