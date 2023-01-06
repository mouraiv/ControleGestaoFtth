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
            Construtora db = CarregarId(construtora.Id);

            if (db == null) throw new Exception("Houve um erro na atualização");

            db.Id = construtora.Id;
   
            _context.Construtoras.Update(db);
            _context.SaveChanges();

            return db;
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

        public IEnumerable<Construtora> Listar(int? pagina, string estacao)
        {
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);

            var result = _context.Construtoras
                .AsNoTracking()
                .Where(p => p.Estacao.NomeEstacao == estacao)
                .Select(value => new Construtora
                {
                    Estacao = value.Estacao,
                    CDO = value.CDO,
                    Cabo = value.Cabo,
                    TotalUms = value.TotalUms,
                    DatadeRecebimento = value.DatadeRecebimento,
                    DatadoTeste = value.DatadoTeste,
                    DatadeConstrucao = value.DatadeConstrucao

                }).ToList().ToPagedList(paginaNumero, paginaTamanho);
            
            return result;

        }

        public IEnumerable<Estacoe> ListarEstacao()
        {
            return _context.Estacoes;
        }
    }
}
