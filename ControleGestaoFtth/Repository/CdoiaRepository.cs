using ControleGestaoFtth.Context;
using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace ControleGestaoFtth.Repository
{
    public class CdoiaRepository : ICdoiaRepository
    {
        private readonly AppDbContext _context;
        public CdoiaRepository(AppDbContext context)
        {
            _context = context;
        }

        public Cdoia Atualizar(Cdoia cdoia)
        {
            Cdoia db = CarregarId(cdoia.Id);

            if (db == null) throw new Exception("Houve um erro na atualização");

            db.Nome = cdoia.Nome;
            db.Status = cdoia.Status;
            db.Observacao = cdoia.Observacao;
            db.AnaliseId = cdoia.AnaliseId;

            _context.Cdoias.Update(db);
            _context.SaveChanges();

            return db;
        }

        public Cdoia Cadastrar(Cdoia cdoia)
        {
            _context.Cdoias.Add(cdoia);
            _context.SaveChanges();
            return cdoia;
        }

        public Cdoia CarregarId(int id)
        {
            return _context.Cdoias
                    .Where(p => p.Id == id)
                    .FirstOrDefault() ?? new Cdoia();
        }

        public bool Deletar(int id)
        {
            Cdoia db = CarregarId(id);

            if (db == null) throw new Exception("Houve um erro ao apagar");

            _context.Cdoias.Remove(db);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<Cdoia> Listar(int analiseId)
        {
            return _context.Cdoias
               .Where(p => p.AnaliseId == analiseId)
               .Select(value => new Cdoia
               {
                   Id = value.Id,
                   Nome = value.Nome,
                   Status= value.Status,
                   Observacao = value.Observacao

               }).OrderBy(p => p.Id)
               .ToList();
        }
    }
}
