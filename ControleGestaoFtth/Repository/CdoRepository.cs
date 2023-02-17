using ControleGestaoFtth.Context;
using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace ControleGestaoFtth.Repository
{
    public class CdoRepository : ICdoRepository
    {
        private readonly AppDbContext _context;
        public CdoRepository(AppDbContext context)
        {
            _context = context;
        }
        public Cdo Atualizar(Cdo cdo)
        {
            Cdo db = CarregarId(cdo.Id);

            if (db == null) throw new Exception("Houve um erro na atualização");

            db.EstacoesId = cdo.EstacoesId;
            db.TipoObraId = cdo.TipoObraId;
            db.Cabo = cdo.Cabo;
            db.Celula = cdo.Celula;
            db.Capacidade = cdo.Capacidade;
            db.TotalUms = cdo.TotalUms;
            db.Endereco = cdo.Endereco;
         
            _context.Cdos.Update(db);
            _context.SaveChanges();

            return db;
        }

        public Cdo Cadastrar(Cdo cdo)
        {
            _context.Cdos.Add(cdo);
            _context.SaveChanges();
            return cdo;
        }

        public Cdo CarregarId(int id)
        {
            return _context.Cdos
                      .Include(p => p.Estacao)  
                      .Include(p => p.TipoObra)
                      .Where(p => p.Id == id)
                      .First();
        }

        public bool Deletar(int id)
        {
            Cdo db = CarregarId(id);

            if (db == null) throw new Exception("Houve um erro ao apagar");

            _context.Cdos.Remove(db);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<Cdo> Listar(int? pagina, string estacao, string cdo, int? cabo, int? celula)
        {
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);

            IQueryable<Cdo> resultado = _context.Cdos
                .AsNoTracking()
                .Include(p => p.Estacao)
                .Select(value => new Cdo
                {
                    Id = value.Id,
                    Estacao = value.Estacao,
                    TipoObra= value.TipoObra,
                    CDO = value.CDO,
                    Cabo = value.Cabo,
                    Celula = value.Celula,
                    Capacidade = value.Capacidade,
                    TotalUms = value.TotalUms,
                    Endereco= value.Endereco
                   
                });

            if (!string.IsNullOrEmpty(estacao) && cdo == null && cabo == null && celula == null)
            {
                return resultado
                    .Where(p => p.Estacao.NomeEstacao.Equals(estacao))
                    .ToList().ToPagedList(paginaNumero, paginaTamanho);
            }
            else if (cdo != null)
            {
                return resultado
                   .Where(p => p.Estacao.NomeEstacao.Equals(estacao) && p.CDO.Equals(cdo))
                   .ToList().ToPagedList(paginaNumero, paginaTamanho);
            }
            else if (cabo != null && celula == null)
            {
                return resultado
                   .Where(p => p.Estacao.NomeEstacao.Equals(estacao) && p.Cabo == cabo)
                   .ToList().ToPagedList(paginaNumero, paginaTamanho);
            }
            else if (celula != null && cabo == null)
            {
                return resultado
                   .Where(p => p.Estacao.NomeEstacao.Equals(estacao) && p.Celula == celula)
                   .ToList().ToPagedList(paginaNumero, paginaTamanho);
            }
            else if (cabo != null && celula != null)
            {
                return resultado
                   .Where(p => p.Estacao.NomeEstacao.Equals(estacao) && p.Cabo == cabo && p.Celula == celula)
                   .ToList().ToPagedList(paginaNumero, paginaTamanho);
            }

            return resultado
                .ToList().ToPagedList(paginaNumero, paginaTamanho);

        }
        public IEnumerable<Estacoe> Estacoes()
        {
            return _context.Estacoes
                .AsNoTracking()
                .Select(value => new Estacoe
                {
                    Id = value.Id,
                    Responsavel = value.Responsavel,
                    NomeEstacao = value.NomeEstacao,

                }).OrderBy(p => p.NomeEstacao)
                .ToList();
        }

        public IEnumerable<Cdo> FilterCdo(string estacao)
        {
            return _context.Cdos
                .AsNoTracking()
                .Where(p => p.Estacao.NomeEstacao == estacao)
                .AsEnumerable()
                .Select(value => new Cdo
                {
                    CDO = value.CDO,

                }).DistinctBy(p => p.CDO)
                .OrderBy(p => p.CDO)
                .ToList();
        }

        public IEnumerable<Cdo> FilterCabo(string estacao)
        {
            return _context.Cdos
               .AsNoTracking()
               .Where(p => p.Estacao.NomeEstacao == estacao)
               .AsEnumerable()
               .Select(value => new Cdo
               {
                   Cabo = value.Cabo,

               }).DistinctBy(p => p.Cabo)
               .OrderBy(p => p.Cabo)
               .ToList();

        }

        public IEnumerable<Cdo> FilterCelula(string estacao)
        {
            return _context.Cdos
                .AsNoTracking()
                .Where(p => p.Estacao.NomeEstacao == estacao)
                .AsEnumerable()
                .Select(value => new Cdo
                {
                    Celula = value.Celula

                }).DistinctBy(p => p.Celula)
                .OrderBy(p => p.Celula)
                .ToList();
        }

        public IEnumerable<TipoObra> TipoObras()
        {
            return _context.TipoObras
                .AsNoTracking()
                .AsEnumerable()
                .DistinctBy(p => p.Nome)
                .OrderBy(p => p.Nome)
                .ToList();
        }

        public IEnumerable<Cdo> UniqueCdo()
        {
            return _context.Cdos
                .AsNoTracking()
                .Select(value => new Cdo
                {
                    CDO = value.CDO,

                }).ToList();
        }
    }
}