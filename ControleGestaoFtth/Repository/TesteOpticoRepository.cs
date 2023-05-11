using Aspose.Pdf.Operators;
using ControleGestaoFtth.Context;
using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
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
                      .ThenInclude(p => p.Estado)
                      .ThenInclude(p => p.Regiao)
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

        public IEnumerable<TesteOptico> Listar(int? pagina, string? regiao, string estado, string? estacao, string cdo, int? cabo, int? celula)
        {
                int paginaTamanho = 10;
                int paginaNumero = (pagina ?? 1);

            var resultado = _context.TesteOpticos
               .Include(p => p.Estacao)
               .ThenInclude(p => p.Estado)
               .ThenInclude(p => p.Regiao)
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
                       Tecnico = value.Tecnico

                   }).AsQueryable();

            if (!string.IsNullOrEmpty(regiao) && string.IsNullOrEmpty(estado) && string.IsNullOrEmpty(estacao))
            {
                 resultado = resultado
                     .Where(p => p.Estacao.Estado.Regiao.Nome.Contains(regiao));                
            }
            else if (!string.IsNullOrEmpty(regiao) && !string.IsNullOrEmpty(estado) && string.IsNullOrEmpty(estacao))
            {
                resultado = resultado
                     .Where(p => p.Estacao.Estado.Regiao.Nome.Contains(regiao) && p.Estacao.Estado.Nome == estado);
            }
            else if (string.IsNullOrEmpty(regiao) && !string.IsNullOrEmpty(estado) && string.IsNullOrEmpty(estacao))
            {
                resultado = resultado
                     .Where(p => p.Estacao.Estado.Nome == estado);
            }
            else if (!string.IsNullOrEmpty(estado) && !string.IsNullOrEmpty(estacao) && string.IsNullOrEmpty(cdo) && cabo == null && celula == null)
            {
                resultado = resultado
                    .Where(p => p.Estacao.Estado.Nome == estado && p.Estacao.NomeEstacao == estacao);
            }
            else if (!string.IsNullOrEmpty(cdo))
            {
                resultado = resultado
                    .Where(p => p.Estacao.NomeEstacao == estacao && p.CDO == cdo);
            }
            else if (cabo != null && celula == null)
            {
                resultado = resultado
                   .Where(p => p.Cabo == cabo);
            }
            else if (celula != null && cabo ==null)
            {
                resultado = resultado
                   .Where(p => p.Celula == celula);
            }
            else if (cabo != null && celula != null)
            {
                resultado = resultado
                   .Where(p => p.Cabo == cabo && p.Celula == celula);
            }

            return resultado
                       .OrderByDescending(x => x.Id)
                       .ToList().ToPagedList(paginaNumero, paginaTamanho);
        }
        public IEnumerable<Estacoe> Estacoes()
        {
            return _context.Estacoes
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
                .ToList();
        }

        public IEnumerable<TipoObra> TipoObras()
        {
            return _context.TipoObras
                .AsEnumerable()
                .DistinctBy(p => p.Nome)
                .OrderBy(p => p.Nome)
                .ToList();
        }

        public IEnumerable<EstadoCampo> EstadoCampos()
        {
            return _context.EstadoCampos
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
        public string GetArquivo(string? caminho)
        {
            string? arquivo = Path.GetDirectoryName(caminho);
                
            return arquivo ?? "";
        }

        public Enderecostotais Enderecototais(string cdo, string municipio, string uf)
        {
            return _context.Enderecostotais
                .AsNoTracking()
                .Where(p => p.MUNICIPIO == municipio &&
                            p.UF == uf)
                .Select(value => new Enderecostotais
                {
                    COD_VIABILIDADE = value.COD_VIABILIDADE

                })
                .FirstOrDefault(p => p.NOME_CDO == cdo) ?? new Enderecostotais();
        }
        public int LastId()
        {
            return _context.TesteOpticos.Max(c => c.Id);
        }
        public IEnumerable<Regioe> Regiao()
        {
            return _context.Regioes
               .AsNoTracking()
               .Select(value => new Regioe
               {
                   Id = value.Id,
                   Nome = value.Nome

               })
               .OrderBy(p => p.Nome)
               .ToList();
        }
        public IEnumerable<Estado> Estado()
        {
            return _context.Estados
                .AsNoTracking()
                .Select(value => new Estado
                {
                    Id = value.Id,
                    Nome = value.Nome

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
                    Nome = value.Nome

                }).OrderBy(p => p.Nome)
                .ToList();
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

        public Materiais Materiais(string cdo, string municipio, string uf)
        {
                var materiais = _context.Materiais
                    .AsNoTracking()
                    .Where(p => p.Municipio.Contains(municipio) &&
                                p.NomeUnidFederativa.Contains(uf))
                    .Select(value => new Materiais
                    {
                        Fabricante = value.Fabricante,
                        Modelo = value.Modelo,
                        EstadoOperacional = value.EstadoOperacional,
                        DataEstadoOperacional = value.DataEstadoOperacional,
                        EstadoControle = value.EstadoControle,
                        DataEstadoControle = value.DataEstadoControle,
                        Infraestrutura = value.Infraestrutura,
                        Endereco = value.Endereco

                    })
                    .FirstOrDefault(p => p.Infraestrutura.Contains(cdo)) ?? new Materiais();
                    
                return materiais;
        }
    }
}