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

            db.EstacoesId = construtora.EstacoesId;
            db.TipoObraId = construtora.TipoObraId;
            db.NetwinId= construtora.NetwinId;
            db.Cabo = construtora.Cabo;
            db.Celula = construtora.Celula;
            db.Capacidade = construtora.Capacidade;
            db.Tecnico = construtora.Tecnico;
            db.TotalUms = construtora.TotalUms;
            db.EquipedeConstrucao = construtora.EquipedeConstrucao;
            db.EstadoCamposId = construtora.EstadoCamposId;
            db.DatadoTeste = construtora.DatadoTeste;
            db.DatadeConstrucao = construtora.DatadeConstrucao;
            db.DatadeRecebimento = construtora.DatadeRecebimento;
            db.Endereco = construtora.Endereco;
            db.BobinadeLancamento = construtora.BobinadeLancamento;
            db.PosicaoICX_DGO = construtora.PosicaoICX_DGO;
            db.BobinadeRecepcao = construtora.BobinadeRecepcao;
            db.SplitterCEOS = construtora.SplitterCEOS;
            db.QuantidadeDeTeste = construtora.QuantidadeDeTeste;
            db.FibraDGO = construtora.FibraDGO;
            db.Observacoes = construtora.Observacoes;

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
            var carregarId = _context.Construtoras
                .Include(p => p.Estacao)
                .Include(p => p.TipoObra)
                .Include(p => p.Netwin)
                .Include(p => p.EstadoCampo)
                .FirstOrDefault(p => p.Id == id);

            return carregarId;
        }

        public bool Deletar(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Construtora> Listar(int? pagina, string estacao, string cdo, int? cabo, int? celula)
        {
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);

            IQueryable<Construtora> resultado = _context.Construtoras
                .AsNoTracking()
                .Include(p => p.Estacao)
                .Include(p => p.Netwin)
                .Select(value => new Construtora
                {
                    Id = value.Id,
                    Estacao = value.Estacao,
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
                    Responsavel = value.Responsavel.Equals(DBNull.Value) ? null : value.Responsavel,
                    NomeEstacao = value.NomeEstacao,

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
                   Nome = value.Nome.Equals(DBNull.Value) ? null : (string) value.Nome

                })
                .OrderBy(p => p.Nome)
                .ToList();
        }

        public IEnumerable<Construtora> UniqueCdo()
        {
            return _context.Construtoras
                .AsNoTracking()
                .Select(value => new Construtora
                {
                    CDO = value.CDO,

                }).ToList();
        }
    }
}
