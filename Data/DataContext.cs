using Alpha.Models;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Plano> Planos { get; set; }
        public DbSet<TipoBeneficio> TipoBeneficios { get; set; }
        public DbSet<Participante> Participantes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer("Server=FabioNote;Database=APOLO;Integrated Security=SSPI;TrustServerCertificate=True");
    }
}
#pragma warning restore format