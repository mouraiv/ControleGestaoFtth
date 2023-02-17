using ControleGestaoFtth.Context;
using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace ControleGestaoFtth.Repository
{
    public class TesteOpticoRepository : ITesteOpticoRepository
    {
        private readonly AppDbContext _context;
        public TesteOpticoRepository(AppDbContext context)
        {
            _context = context;
        }

        public TesteOptico Atualizar(TesteOptico testeOptico)
        {
            TesteOptico db = CarregarId(testeOptico.Id);

            if (db == null) throw new Exception("Houve um erro na atualização");

            db.NetwinId = testeOptico.NetwinId;
            db.Tecnico = testeOptico.Tecnico;
            db.EquipedeConstrucao = testeOptico.EquipedeConstrucao;
            db.EstadoCamposId = testeOptico.EstadoCamposId;
            db.DatadoTeste = testeOptico.DatadoTeste;
            db.DatadeConstrucao = testeOptico.DatadeConstrucao;
            db.DatadeRecebimento = testeOptico.DatadeRecebimento;
            db.BobinadeLancamento = testeOptico.BobinadeLancamento;
            db.PosicaoICX_DGO = testeOptico.PosicaoICX_DGO;
            db.BobinadeRecepcao = testeOptico.BobinadeRecepcao;
            db.SplitterCEOS = testeOptico.SplitterCEOS;
            db.QuantidadeDeTeste = testeOptico.QuantidadeDeTeste;
            db.FibraDGO = testeOptico.FibraDGO;
            db.Observacoes = testeOptico.Observacoes;
            db.CdosId = testeOptico.CdosId;

            _context.TesteOpticos.Update(db);
            _context.SaveChanges();

            return db;
        }

        public TesteOptico Cadastrar(TesteOptico testeOptico)
        {
            _context.TesteOpticos.Add(testeOptico);
            _context.SaveChanges();
            return testeOptico;
        }

        public TesteOptico CarregarId(int id)
        {
            return _context.TesteOpticos
                      .Include(p => p.Netwin)
                      .Include(p => p.EstadoCampo)
                      .Where(p => p.Id == id)
                      .First();
        }

        public bool Deletar(int id)
        {
            TesteOptico db = CarregarId(id);

            if (db == null) throw new Exception("Houve um erro ao apagar");

            _context.TesteOpticos.Remove(db);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<TesteOptico> Listar(int? pagina, string estacao, string cdo, int? cabo, int? celula)
        {
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);

            IQueryable<TesteOptico> resultado = _context.TesteOpticos
                .AsNoTracking()
                .Include(p => p.Cdo)
                .Include(p => p.Netwin)
                .Select(value => new TesteOptico
                {
                    Id = value.Id,
                    Cdo = value.Cdo,
                    Netwin = value.Netwin,
                    DatadeRecebimento = value.DatadeRecebimento,
                    State = value.State,
                    DatadoTeste = value.DatadoTeste,
                    DatadeConstrucao = value.DatadeConstrucao,
                    EquipedeConstrucao = value.EquipedeConstrucao,
                    Tecnico = value.Tecnico,
                });

            if (!string.IsNullOrEmpty(estacao) && cdo == null && cabo == null && celula == null)
            {
                return resultado
                    .Where(p => p.Cdo.Estacao.NomeEstacao.Equals(estacao))
                    .ToList().ToPagedList(paginaNumero, paginaTamanho);
            }
            else if (cdo != null)
            {
                return resultado
                   .Where(p => p.Cdo.Estacao.NomeEstacao.Equals(estacao) && p.Cdo.CDO.Equals(cdo))
                   .ToList().ToPagedList(paginaNumero, paginaTamanho);
            }
            else if (cabo != null && celula == null)
            {
                return resultado
                   .Where(p => p.Cdo.Estacao.NomeEstacao.Equals(estacao) && p.Cdo.Cabo == cabo)
                   .ToList().ToPagedList(paginaNumero, paginaTamanho);
            }
            else if (celula != null && cabo == null)
            {
                return resultado
                   .Where(p => p.Cdo.Estacao.NomeEstacao.Equals(estacao) && p.Cdo.Celula == celula)
                   .ToList().ToPagedList(paginaNumero, paginaTamanho);
            }
            else if (cabo != null && celula != null)
            {
                return resultado
                   .Where(p => p.Cdo.Estacao.NomeEstacao.Equals(estacao) && p.Cdo.Cabo == cabo && p.Cdo.Celula == celula)
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

        public IEnumerable<Netwin> Netwins()
        {
            return _context.Netwins
                .AsNoTracking()
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

        public IEnumerable<EstadoCampo> EstadoCampos()
        {
            return _context.EstadoCampos
                .AsNoTracking()
                .Select(value => new EstadoCampo
                {
                    Id = value.Id,
                    Nome = value.Nome

                })
                .OrderBy(p => p.Nome)
                .ToList();
        }

        public IEnumerable<TesteOptico> Listar()
        {
            return _context.TesteOpticos
               .AsNoTracking()
               .ToList();
        }
    }
}