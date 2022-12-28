using ControleGestaoFtth.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleGestaoFtth.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 
        }

        public DbSet<Construtora> construtoras { get; set; }
        public DbSet<Estacoe> estacoes { get; set; }
        public DbSet<EstadoCampo> EstadoCampos { get; set; }
        public DbSet<Netwin> netwins { get; set; }
        public DbSet<State> states { get; set; }
        public DbSet<Tecnico> tecnicos { get; set;}
        public DbSet<TipoObra> tipoObras { get; set; }
        public DbSet<Usuario> usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
