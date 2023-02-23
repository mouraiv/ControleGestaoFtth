using ControleGestaoFtth.Context;
using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace ControleGestaoFtth.Repository
{
    public class TipoObraRepository : ITipoObraRepository
    {
        private readonly AppDbContext _context;
        public TipoObraRepository(AppDbContext context)
        {
            _context = context;
        }
        public TipoObra Atualizar(TipoObra tipoObra)
        {
            TipoObra db = CarregarId(tipoObra.Id);

            if (db == null) throw new Exception("Houve um erro na atualização");

            db.Nome = tipoObra.Nome;

            _context.TipoObras.Update(db);
            _context.SaveChanges();

            return db;
        }

        public TipoObra Cadastrar(TipoObra tipoObra)
        {
            _context.TipoObras.Add(tipoObra);
            _context.SaveChanges();
            return tipoObra;
        }

        public TipoObra CarregarId(int id)
        {
            return _context.TipoObras
                    .Where(p => p.Id == id)
                    .First();
        }

        public bool Deletar(int id)
        {
            TipoObra db = CarregarId(id);

            if (db == null) throw new Exception("Houve um erro ao apagar");

            _context.TipoObras.Remove(db);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<TipoObra> Listar()
        {
            return _context.TipoObras
                .AsNoTracking()
                .Select(value => new TipoObra
                {
                    Id = value.Id,
                    Nome = value.Nome,

                }).OrderBy(p => p.Nome)
                .ToList();
        }

        public IEnumerable<TipoObra> Listar(int? pagina, string nome)
        {
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);

            IQueryable<TipoObra> resultado = _context.TipoObras.AsNoTracking();

            if (nome != null)
            {
                return resultado.
                    Where(p => p.Nome.Equals(nome))
                   .ToList().ToPagedList(paginaNumero, paginaTamanho);
            }

            return resultado
                .ToList().ToPagedList(paginaNumero, paginaTamanho);
        }

        public IEnumerable<TipoObra> Obras()
        {
            return _context.TipoObras
                .AsNoTracking()
                .AsEnumerable()
                .Select(value => new TipoObra
                {
                    Id = value.Id,
                    Nome = value.Nome

                }).DistinctBy(p => p.Nome)
                .OrderBy(p => p.Nome)
                .ToList();
        }

        public IEnumerable<TesteOptico> UniqueFk()
        {
            return _context.TesteOpticos
               .AsNoTracking()
               .Select(value => new TesteOptico
               {
                   TipoObraId = value.TipoObraId

               }).ToList();
        }
    }
}