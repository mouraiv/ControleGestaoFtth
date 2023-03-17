using ControleGestaoFtth.Context;
using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO;
using System.Security.Policy;
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

        public TesteOptico Atualizar(TesteOptico TesteOptico)
        {
            TesteOptico db = CarregarId(TesteOptico.Id);

            if (db == null) throw new Exception("Houve um erro na atualização");

            db.EstacoesId = TesteOptico.EstacoesId;
            db.TipoObraId = TesteOptico.TipoObraId;
            db.NetwinId = TesteOptico.NetwinId;
            db.ConstrutorasId = TesteOptico.ConstrutorasId;
            db.Cabo = TesteOptico.Cabo;
            db.Celula = TesteOptico.Celula;
            db.Capacidade = TesteOptico.Capacidade;
            db.Tecnico = TesteOptico.Tecnico;
            db.TotalUms = TesteOptico.TotalUms;
            db.EquipedeConstrucao = TesteOptico.EquipedeConstrucao;
            db.EstadoCamposId = TesteOptico.EstadoCamposId;
            db.DatadoTeste = TesteOptico.DatadoTeste;
            db.DatadeConstrucao = TesteOptico.DatadeConstrucao;
            db.DatadeRecebimento = TesteOptico.DatadeRecebimento;
            db.Endereco = TesteOptico.Endereco;
            db.BobinadeLancamento = TesteOptico.BobinadeLancamento;
            db.PosicaoICX_DGO = TesteOptico.PosicaoICX_DGO;
            db.BobinadeRecepcao = TesteOptico.BobinadeRecepcao;
            db.SplitterCEOS = TesteOptico.SplitterCEOS;
            db.QuantidadeDeTeste = TesteOptico.QuantidadeDeTeste;
            db.FibraDGO = TesteOptico.FibraDGO;
            db.Observacoes = TesteOptico.Observacoes;

            _context.TesteOpticos.Update(db);
            _context.SaveChanges();

            return db;
        }

        public TesteOptico Cadastrar(TesteOptico TesteOptico)
        {
            _context.TesteOpticos.Add(TesteOptico);
            _context.SaveChanges();
            return TesteOptico;
        }

        public TesteOptico CarregarId(int id)
        {
            return _context.TesteOpticos
                      .Include(p => p.Estacao)
                      .Include(p => p.Construtora)
                      .Include(p => p.TipoObra)
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
                .Include(p => p.Estacao)
                .Include(p => p.Construtora)
                .Include(p => p.Netwin)
                .Select(value => new TesteOptico
                {
                    Id = value.Id,
                    Estacao = value.Estacao,
                    Construtora = value.Construtora,
                    CDO = value.CDO,
                    Cabo = value.Cabo,
                    Celula = value.Celula,
                    TotalUms = value.TotalUms,
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
                    NomeEstacao = value.NomeEstacao,

                }).OrderBy(p => p.NomeEstacao)
                .ToList();
        }

        public IEnumerable<TesteOptico> FilterCdo(string estacao)
        {
            return _context.TesteOpticos
                .AsNoTracking()
                .Include(p => p.Estacao)
                .Where(p => p.Estacao.NomeEstacao == estacao)
                .AsEnumerable()
                .Select(value => new TesteOptico
                {
                    CDO = value.CDO,

                }).DistinctBy(p => p.CDO)
                .OrderBy(p => p.CDO)
                .ToList();
        }

        public IEnumerable<TesteOptico> FilterCabo(string estacao)
        {
            return _context.TesteOpticos
               .AsNoTracking()
               .Include(p => p.Estacao)
               .Where(p => p.Estacao.NomeEstacao == estacao)
               .AsEnumerable()
               .Select(value => new TesteOptico
               {
                   Cabo = value.Cabo,

               }).DistinctBy(p => p.Cabo)
               .OrderBy(p => p.Cabo)
               .ToList();

        }

        public IEnumerable<TesteOptico> FilterCelula(string estacao)
        {
            return _context.TesteOpticos
                .AsNoTracking()
                .Include(p => p.Estacao)
                .Where(p => p.Estacao.NomeEstacao == estacao)
                .AsEnumerable()
                .Select(value => new TesteOptico
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

        public IEnumerable<TesteOptico> UniqueCdo()
        {
            return _context.TesteOpticos
                .AsNoTracking()
                .Select(value => new TesteOptico
                {
                    CDO = value.CDO,

                }).ToList();
        }

        public IEnumerable<Construtora> Construtoras()
        {
            return _context.Construtoras
                .AsNoTracking()
                .Select(value => new Construtora
                {
                    Id = value.Id,
                    Nome = value.Nome

                }).OrderBy(p => p.Nome)
                .ToList();
        }

        public List<string> ArquivoOptico(string sgl, string cdo, string[] extensoes)
        {
            string pasta = "Upload\\TesteOptico\\Anexos\\"+sgl+"\\TESTE_OPTICO\\"+cdo+"\\";

            var arquivo = Directory.GetFiles(pasta, "*", SearchOption.AllDirectories)
                         .Where(file => extensoes.Contains(Path.GetExtension(file)))
                         .ToList();

            return arquivo;
        }
    }
}