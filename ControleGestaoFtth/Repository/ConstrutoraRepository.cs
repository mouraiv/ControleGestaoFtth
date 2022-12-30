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
        public Construtora Atualizar(Construtora construtora)
        {
            throw new NotImplementedException();
        }

        public Construtora Cadastrar(Construtora construtora)
        {
            throw new NotImplementedException();
        }

        public Construtora CarregarId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Construtora> Listar()
        {
            throw new NotImplementedException();
        }

        public bool Deletar(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Construtora> Listar(int? pagina)
        {
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);

            return _context.construtoras
                .Include(p => p.Estacao)
                .Include(p => p.TipoObra)
                .Include(p => p.EstadoCampo)
                .Include(p => p.State)
                    .ToList().ToPagedList(paginaNumero, paginaTamanho);
        }
    }
}
