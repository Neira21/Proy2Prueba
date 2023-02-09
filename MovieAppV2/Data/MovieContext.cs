using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieAppV2.Models;

namespace MovieAppV2.Data
{
    public class MovieContext : IdentityDbContext
    {
        public MovieContext (DbContextOptions<MovieContext> dbo)
            : base(dbo)
        {

        }
        public DbSet<Usuario> Usuarios {get; set;}
        public DbSet<Pelicula> Peliculas {get; set;}
        public DbSet<Noticia> Noticias {get; set;}
        public DbSet<Reseña> Reseñas {get; set;}
    }
}