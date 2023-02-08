using ControleGestaoFtth.Context;
using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace ControleGestaoFtth.Repository
{
    public class EstadoCampoRepository : IEstadoCampoRepository
    {
        private readonly AppDbContext _context;
        public EstadoCampoRepository(AppDbContext context)
        {
            _context = context;
        }
        public EstadoCampo Atualizar(EstadoCampo EstadoCampo)
        {
            EstadoCampo db = CarregarId(EstadoCampo.Id);

            if (db == null) throw new Exception("Houve um erro na atualização");

            db.Nome = EstadoCampo.Nome;

            _context.EstadoCampos.Update(db);
            _context.SaveChanges();

            return db;
        }

        public EstadoCampo Cadastrar(EstadoCampo estadoCampo)
        {
            _context.EstadoCampos.Add(estadoCampo);
            _context.SaveChanges();
            return estadoCampo;
        }

        public EstadoCampo CarregarId(int id)
        {
            return _context.EstadoCampos
                    .Where(p => p.Id == id)
                    .First();
        }

        public bool Deletar(int id)
        {
            EstadoCampo db = CarregarId(id);

            if (db == null) throw new Exception("Houve um erro ao apagar");

            _context.EstadoCampos.Remove(db);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<EstadoCampo> Listar()
        {
            return _context.EstadoCampos
                .AsNoTracking()
                .Select(value => new EstadoCampo
                {
                    Id = value.Id,
                    Nome = value.Nome,

                }).OrderBy(p => p.Nome)
                .ToList();
        }

        public IEnumerable<EstadoCampo> Listar(int? pagina, string nome)
        {
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);

            IQueryable<EstadoCampo> resultado = _context.EstadoCampos.AsNoTracking();

            if (nome != null)
            {
                return resultado.
                    Where(p => p.Nome.Equals(nome))
                   .ToList().ToPagedList(paginaNumero, paginaTamanho);
            }

            return resultado
                .ToList().ToPagedList(paginaNumero, paginaTamanho);
        }

        public IEnumerable<EstadoCampo> Campo()
        {
            return _context.EstadoCampos
                .AsNoTracking()
                .AsEnumerable()
                .Select(value => new EstadoCampo
                {
                    Id = value.Id,
                    Nome = value.Nome

                }).DistinctBy(p => p.Nome)
                .OrderBy(p => p.Nome)
                .ToList();
        }

        public IEnumerable<Construtora> UniqueFk()
        {
            return _context.Construtoras
               .AsNoTracking()
               .Select(value => new Construtora
               {
                   EstadoCamposId = value.EstadoCamposId

               }).ToList();
        }
    }
}