using Microsoft.EntityFrameworkCore;

namespace mf_apis_web_services_fuel_manager.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<VeiculoUsuario>()
                .HasKey(c => new { c.VeiculoId, c.UsuarioId });

            builder.Entity<VeiculoUsuario>()
                .HasOne(c => c.Veiculo).WithMany(c => c.Usuarios)
                .HasForeignKey(c => c.VeiculoId);

            builder.Entity<VeiculoUsuario>()
                .HasOne(c => c.Usuario).WithMany(c => c.Veiculos)
                .HasForeignKey(c => c.UsuarioId);
        }
        public DbSet<Veiculo> Veiculo { get; set; }
        public DbSet<Consumo> Consumo { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<VeiculoUsuario> VeiculoUsuario { get; set; }
    }
}
