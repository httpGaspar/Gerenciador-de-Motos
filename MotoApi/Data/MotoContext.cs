using Microsoft.EntityFrameworkCore;
using MotoApi.Models;

namespace MotoApi.Data
{
    public class MotoContext : DbContext
    {
        public MotoContext(DbContextOptions<MotoContext> options) : base(options) { }

        public DbSet<Moto> Motos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Definir a placa como única
            modelBuilder.Entity<Moto>()
                .HasIndex(m => m.Placa)
                .IsUnique();

            // Configurar outras propriedades, se necessário
        }
    }
}
