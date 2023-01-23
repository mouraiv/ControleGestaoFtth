using ControleGestaoFtth.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleGestaoFtth.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 
        }

        public DbSet<Construtora> Construtoras => Set<Construtora>();
        public DbSet<Estacoe> Estacoes => Set<Estacoe>();
        public DbSet<EstadoCampo> EstadoCampos => Set<EstadoCampo>();
        public DbSet<Netwin> Netwins => Set<Netwin>();
        public DbSet<State> States => Set<State>();
        public DbSet<Tecnico> Tecnicos => Set<Tecnico>();
        public DbSet<TipoObra> TipoObras => Set<TipoObra>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Construtora>()
                .HasOne(p => p.Estacao)
                .WithMany()
                .HasForeignKey(p => p.EstacoesId);

            modelBuilder.Entity<Construtora>()
                .HasOne(p => p.TipoObra)
                .WithMany()
                .HasForeignKey(p => p.TipoObraId);

            modelBuilder.Entity<Construtora>()
                .HasOne(p => p.EstadoCampo)
                .WithMany()
                .HasForeignKey(p => p.EstadoCamposId);

            modelBuilder.Entity<Construtora>()
                .HasOne(p => p.State)
                .WithMany()
                .HasForeignKey(p => p.StatesId);

            modelBuilder.Entity<Construtora>()
                .HasOne(p => p.Netwin)
                .WithMany()
                .HasForeignKey(p => p.NetwinId);

            modelBuilder.Entity<Tecnico>()
                .HasOne(p => p.usuario)
                .WithMany()
                .HasForeignKey(p => p.UsuarioId);
        }
    }
}
