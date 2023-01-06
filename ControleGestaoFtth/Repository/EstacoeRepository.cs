using ControleGestaoFtth.Context;
using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;

namespace ControleGestaoFtth.Repository
{
    public class EstacoeRepository : IEstacoeRepository
    {
        private readonly AppDbContext _context;
        public EstacoeRepository(AppDbContext context)
        {
            _context = context;
        }
        public Estacoe Atualizar(Estacoe construtora)
        {
            throw new NotImplementedException();
        }

        public Estacoe Cadastrar(Estacoe construtora)
        {
            throw new NotImplementedException();
        }

        public Estacoe CarregarId(int id)
        {
            throw new NotImplementedException();
        }

        public bool Deletar(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Estacoe> Listar()
        {
            throw new NotImplementedException();
        }
    }
}
