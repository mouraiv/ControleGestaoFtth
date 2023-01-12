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

        public bool Deletar(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Construtora> Listar(int? pagina, string estacao, string cdo)
        {
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);
            
            if(estacao != null)
            {
                return _context.Construtoras
                .AsNoTracking()
                .Where(p => p.Estacao.NomeEstacao == estacao || p.CDO == cdo)
                .Select(value => new Construtora
                {
                    Estacao = value.Estacao,
                    CDO = value.CDO,
                    Cabo = value.Cabo,
                    Celula = value.Celula,
                    TotalUms = value.TotalUms,
                    DatadeRecebimento = value.DatadeRecebimento,
                    DatadoTeste = value.DatadoTeste,
                    DatadeConstrucao = value.DatadeConstrucao

                }).ToList().ToPagedList(paginaNumero, paginaTamanho);
            }

            return _context.Construtoras
                .AsNoTracking()
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

        }

        public IEnumerable<Estacoe> Estacoes()
        {
            return _context.Estacoes
                .AsNoTracking()
                .Select(value => new Estacoe
            {
                Id = value.Id,
                NomeEstacao = value.NomeEstacao,
                //Modelo de tratamento DBnull Rows
                //Responsavel = DBNull.Value.Equals(value.Responsavel) ? string.Empty : value.Responsavel

            }).OrderBy(p => p.NomeEstacao)
            .ToList();
        }

        public IEnumerable<Construtora> FilterCdo(string estacao)
        {
            return _context.Construtoras
                .AsNoTracking()
                .Where(p => p.Estacao.NomeEstacao == estacao)
                .AsEnumerable()
                .Select(value => new Construtora
                {
                    CDO = value.CDO,

                }).DistinctBy(p => p.CDO)
                .OrderBy(p => p.CDO)
                .ToList();
        }

        public IEnumerable<Construtora> FilterCabo(string estacao)
        {
            return _context.Construtoras
               .AsNoTracking()
               .Where(p => p.Estacao.NomeEstacao == estacao)
               .AsEnumerable()
               .Select(value => new Construtora
               {
                   Cabo = value.Cabo == null ? 0 : value.Cabo,

               }).DistinctBy(p => p.Cabo)
               .OrderBy(p => p.Cabo)
               .ToList();

        }

        public IEnumerable<Construtora> FilterCelula(string estacao)
        {
            return _context.Construtoras
                .AsNoTracking()
                .Where(p => p.Estacao.NomeEstacao == estacao)
                .AsEnumerable()
                .Select(value => new Construtora
                {
                    Celula = value.Celula

                }).DistinctBy(p => p.Celula)
                .OrderBy(p => p.Celula)
                .ToList();
        }
    }
}
