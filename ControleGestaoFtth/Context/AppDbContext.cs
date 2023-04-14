using ControleGestaoFtth.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleGestaoFtth.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.SetCommandTimeout(1800);
        }

        public DbSet<Construtora> Construtoras => Set<Construtora>();
        public DbSet<TesteOptico> TesteOpticos => Set<TesteOptico>();
        public DbSet<Estacoe> Estacoes => Set<Estacoe>();
        public DbSet<EstadoCampo> EstadoCampos => Set<EstadoCampo>();
        public DbSet<Netwin> Netwins => Set<Netwin>();
        public DbSet<State> States => Set<State>();
        public DbSet<Tecnico> Tecnicos => Set<Tecnico>();
        public DbSet<TipoObra> TipoObras => Set<TipoObra>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Enderecostotais> Enderecostotais => Set<Enderecostotais>();

        public override int SaveChanges()
        {
            // converte todas as propriedades de string em caixa alta antes de salvar
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                foreach (var property in entry.CurrentValues.Properties.Where(p => p.ClrType == typeof(string)))
                {
                    var currentValue = (string?)entry.CurrentValues[property];
                    if (!string.IsNullOrEmpty(currentValue))
                    {
                        entry.CurrentValues[property] = currentValue.ToUpperInvariant();
                    }
                }
            }

            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TesteOptico>()
                .HasOne(p => p.Estacao)
                .WithMany()
                .HasForeignKey(p => p.EstacoesId);

            modelBuilder.Entity<TesteOptico>()
                .HasOne(p => p.TipoObra)
                .WithMany()
                .HasForeignKey(p => p.TipoObraId);

            modelBuilder.Entity<TesteOptico>()
                .HasOne(p => p.EstadoCampo)
                .WithMany()
                .HasForeignKey(p => p.EstadoCamposId);

            modelBuilder.Entity<TesteOptico>()
                .HasOne(p => p.State)
                .WithMany()
                .HasForeignKey(p => p.StatesId);

            modelBuilder.Entity<TesteOptico>()
                .HasOne(p => p.Netwin)
                .WithMany()
                .HasForeignKey(p => p.NetwinId);

            modelBuilder.Entity<TesteOptico>()
                .HasOne(p => p.Construtora)
                .WithMany()
                .HasForeignKey(p => p.ConstrutorasId);

            modelBuilder.Entity<Tecnico>()
                .HasOne(p => p.usuario)
                .WithMany()
                .HasForeignKey(p => p.UsuarioId);
        }
    }
}
