using ControleGestaoFtth.Context;
using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace ControleGestaoFtth.Repository
{
    public class AnaliseRepository : IAnaliseRepository
    {
        private readonly AppDbContext _context;
        public AnaliseRepository(AppDbContext context)
        {
            _context = context;
        }
        public Analise Atualizar(Analise analise)
        {
            Analise db = CarregarId(analise.Id);

            if (db == null) throw new Exception("Houve um erro na atualização");

            db.TesteOpticoId = analise.TesteOpticoId;
            db.Status = analise.Status;
            db.TecnicoId = analise.TecnicoId;
            db.DataAnalise = analise.DataAnalise;
            db.Observacao = analise.Observacao;
            db.CDOIA = analise.CDOIA;
            db.CDOIAStatus = analise.CDOIAStatus;
            db.CDOIA_Obs = analise.CDOIA_Obs;
           
            _context.Analises.Update(db);
            _context.SaveChanges();

            return db;
        }

        public Analise Cadastrar(Analise analise)
        {
            _context.Analises.Add(analise);
            _context.SaveChanges();
            return analise;
        }
        public Analise CarregarIdTesteOptico(int id)
        {
            return _context.Analises
                      .Include(p => p.TesteOptico)
                      .ThenInclude(p => p.Estacao)
                      .ThenInclude(p => p.Estado)
                      .ThenInclude(p => p.Regiao)
                      .Include(p => p.TesteOptico)
                      .ThenInclude(p => p.Construtora)
                      .Include(p => p.TesteOptico)
                      .ThenInclude(p => p.TipoObra)
                      .Include(p => p.Tecnico)
                      .Where(p => p.TesteOpticoId == id)
                      .OrderByDescending(g => g.DataAnalise)
                      .FirstOrDefault() ?? new Analise();
        }
        public Analise CarregarId(int id)
        {
            return _context.Analises
                      .Include(p => p.Tecnico)
                      .Where(p => p.Id == id)
                      .OrderByDescending(g => g.DataAnalise)
                      .FirstOrDefault() ?? new Analise();
        }

        public bool Deletar(int id)
        {
            Analise db = CarregarId(id);

            if (db == null) throw new Exception("Houve um erro ao apagar");

            _context.Analises.Remove(db);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<Estacoe> Estacoes(string estado)
        {
            return _context.Estacoes
                .Include(p => p.Estado)
                .Where(p => p.Estado.Nome == estado)
                .AsEnumerable()
                .Select(value => new Estacoe
                {
                    Id = value.Id,
                    NomeEstacao = value.NomeEstacao,

                }).OrderBy(p => p.NomeEstacao)
                .ToList();
        }

        public IEnumerable<Estado> Estado()
        {
            return _context.Estados
               .AsNoTracking()
               .Select(value => new Estado
               {
                   Id = value.Id,
                   Nome = value.Nome,
                   Regiao = value.Regiao


               }).OrderBy(p => p.Nome)
               .ToList();
        }

        public IEnumerable<Estado> Estado(string regiao)
        {
            return _context.Estados
                .AsNoTracking()
                .Include(p => p.Regiao)
                .Where(p => p.Regiao.Nome.Contains(regiao))
                .AsEnumerable()
                .Select(value => new Estado
                {
                    Id = value.Id,
                    Nome = value.Nome,

                }).OrderBy(p => p.Nome)
                .ToList();
        }

        public IEnumerable<Analise> Listar(int? pagina, string? regiao, string estado, string? estacao, int? cdo, int? tecnico, int? status)
        {
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);

            var resultado = _context.Analises
                .AsNoTracking()
                .Where(a => a.TesteOpticoId != null)
                .Include(p => p.TesteOptico)
                   .ThenInclude(p => p.Estacao)
                   .ThenInclude(p => p.Estado)
                   .ThenInclude(p => p.Regiao)
               .Include(p => p.TesteOptico)
                   .ThenInclude(p => p.Construtora)
               .Include(p => p.Tecnico)
                   .AsEnumerable()
                   .DistinctBy(d => d.TesteOpticoId)
                   .OrderByDescending(a => a.DataAnalise)
                   .Select(value => new Analise
                   {
                       TesteOpticoId = value.TesteOpticoId,
                       Tecnico = value.Tecnico,
                       TesteOptico = value.TesteOptico,
                       Status = value.Status,
       
                   }).AsQueryable();

            if (!string.IsNullOrEmpty(regiao) && string.IsNullOrEmpty(estado) && string.IsNullOrEmpty(estacao))
            {
                resultado = resultado
                    .Where(p => p.TesteOptico.Estacao.Estado.Regiao.Nome.Contains(regiao));
            }
            else if (!string.IsNullOrEmpty(regiao) && !string.IsNullOrEmpty(estado) && string.IsNullOrEmpty(estacao))
            {
                resultado = resultado
                     .Where(p => p.TesteOptico.Estacao.Estado.Regiao.Nome.Contains(regiao) && p.TesteOptico.Estacao.Estado.Nome == estado);
            }
            else if (string.IsNullOrEmpty(regiao) && !string.IsNullOrEmpty(estado) && string.IsNullOrEmpty(estacao))
            {
                resultado = resultado
                     .Where(p => p.TesteOptico.Estacao.Estado.Nome == estado);
            }
            else if (!string.IsNullOrEmpty(estado) && !string.IsNullOrEmpty(estacao) && cdo == null && tecnico == null &&  status == null)
            {
                resultado = resultado
                    .Where(p => p.TesteOptico.Estacao.Estado.Nome == estado && p.TesteOptico.Estacao.NomeEstacao == estacao);
            }
            else if (cdo != null)
            {
                resultado = resultado
                    .Where(p => p.TesteOptico.Estacao.NomeEstacao == estacao && p.TesteOpticoId == cdo);
            }
            else if (tecnico != null && status == null)
            {
                resultado = resultado
                   .Where(p => p.TecnicoId == tecnico);
            }
            else if (status != null && tecnico == null)
            {
                resultado = resultado
                   .Where(p => p.Status == status);
            }
            else if (tecnico != null && status != null)
            {
                resultado = resultado
                   .Where(p => p.TecnicoId == tecnico && p.Status == status);
            }

            return resultado
                   .ToList().ToPagedList(paginaNumero, paginaTamanho);
        }

        public IEnumerable<Regioe> Regiao()
        {
            return _context.Regioes
               .AsNoTracking()
               .Select(value => new Regioe
               {
                   Id = value.Id,
                   Nome = value.Nome,

               })
               .OrderBy(p => p.Nome)
               .ToList();
        }

        public IEnumerable<Tecnico> Tecnico()
        {
            return _context.Tecnicos
               .AsNoTracking()
               .Select(value => new Tecnico
               {
                   Id = value.Id,
                   Nome = value.Nome,

               })
               .OrderBy(p => p.Nome)
               .ToList();
        }

        public IEnumerable<TesteOptico> TesteOptico(string estacao)
        {
            return _context.TesteOpticos
                .AsNoTracking()
                .Include(p => p.Estacao)
                .Where(p => p.Estacao.NomeEstacao == estacao)
                .AsEnumerable()
                .Select(value => new TesteOptico
                {
                    Id = value.Id,
                    CDO = value.CDO,

                }).DistinctBy(p => p.CDO)
                .OrderBy(p => p.CDO)
                .ToList();
        }

        public IEnumerable<Analise> UniqueCdo()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Analise> Historico(int? testeOpticoId)
        {
            return _context.Analises
                .Include(p => p.Tecnico)
                .Where(p => p.TesteOpticoId == testeOpticoId)
                .ToList();
        }
    }
}
