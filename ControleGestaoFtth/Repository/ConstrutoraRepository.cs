using ControleGestaoFtth.Context;
using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace ControleGestaoFtth.Repository
{
    public class ConstrutoraRepository : IConstrutoraRepository
    {
        private readonly AppDbContext _context;
        public ConstrutoraRepository(AppDbContext context)
        {
            _context = context;
        }
        public Construtora Atualizar(Construtora Construtora)
        {
            Construtora db = CarregarId(Construtora.Id);

            if (db == null) throw new Exception("Houve um erro na atualização");

            db.Nome = Construtora.Nome;

            _context.Construtoras.Update(db);
            _context.SaveChanges();

            return db;
        }

        public Construtora Cadastrar(Construtora Construtora)
        {
            _context.Construtoras.Add(Construtora);
            _context.SaveChanges();
            return Construtora;
        }

        public Construtora CarregarId(int id)
        {
            return _context.Construtoras
                    .Where(p => p.Id == id)
                    .First();
        }

        public bool ContrutoraExiste(string nome)
        {
            return _context.Construtoras.Any(p => p.Nome == nome);
        }

        public bool Deletar(int id)
        {
            Construtora db = CarregarId(id);

            if (db == null) throw new Exception("Houve um erro ao apagar");

            _context.Construtoras.Remove(db);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<Construtora> Listar()
        {
            return _context.Construtoras
                .AsNoTracking()
                .Select(value => new Construtora
                {
                    Id = value.Id,
                    Nome = value.Nome,

                }).OrderBy(p => p.Nome)
                .ToList();
        }

        public IEnumerable<Construtora> Listar(int? pagina, string nome)
        {
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);

            IQueryable<Construtora> resultado = _context.Construtoras.AsNoTracking();

            if (nome != null)
            {
                return resultado.
                    Where(p => p.Nome.Equals(nome))
                   .ToList().ToPagedList(paginaNumero, paginaTamanho);
            }

            return resultado
                .ToList().ToPagedList(paginaNumero, paginaTamanho);
        }

        public IEnumerable<Construtora> Obras()
        {
            return _context.Construtoras
                .AsNoTracking()
                .AsEnumerable()
                .Select(value => new Construtora
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
                   ConstrutorasId = value.ConstrutorasId

               }).ToList();
        }
    }
}