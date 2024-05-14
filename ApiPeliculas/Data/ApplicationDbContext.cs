using ApiPeliculas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        //models hear
        public DbSet<Category> Category { get; set; }
        public DbSet<Movie> Movie { get; set; }
    }
}
