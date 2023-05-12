using ControleGestaoFtth.Context;
using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using X.PagedList;

namespace ControleGestaoFtth.Repository
{
    public class EstacoeRepository : IEstacoeRepository
    {
        private readonly AppDbContext _context;
        public EstacoeRepository(AppDbContext context)
        {
            _context = context;
        }
        public Estacoe Atualizar(Estacoe estacao)
        {
            Estacoe db = CarregarId(estacao.Id);

            if (db == null) throw new Exception("Houve um erro na atualização");

            db.EstadosId = estacao.EstadosId;
            db.NomeEstacao = estacao.NomeEstacao;
            db.Sigla = estacao.Sigla;

            _context.Estacoes.Update(db);
            _context.SaveChanges();

            return db;
        }

        public Estacoe Cadastrar(Estacoe estacao)
        {
            _context.Estacoes.Add(estacao);
            _context.SaveChanges();
            return estacao;
        }

        public Estacoe CarregarId(int id)
        {
            return _context.Estacoes
                    .Where(p => p.Id == id)
                    .First();
        }

        public bool Deletar(int id)
        {
            Estacoe db = CarregarId(id);

            if (db == null) throw new Exception("Houve um erro ao apagar");

            _context.Estacoes.Remove(db);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<Estacoe> Listar()
        {
            return _context.Estacoes
                .Select(value => new Estacoe
                {
                    Id = value.Id,
                    NomeEstacao = value.NomeEstacao,

                }).OrderBy(p => p.NomeEstacao)
                .ToList();
        }

        public IEnumerable<Estacoe> Listar(int? pagina, string estado, string estacao)
        {
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);

            var resultado = _context.Estacoes
                .Include(p => p.Estado).AsQueryable();

            if (estado != null && estacao == null)
            {
                resultado = resultado.
                    Where(p => p.Estado.Nome == estado);

            }else if (estado != null && estacao != null)
            {
                resultado = resultado.
                    Where(p => p.Estado.Nome == estado && p.NomeEstacao == estacao);
            }

            return resultado
                .OrderByDescending(x => x.Id)
                .ToList().ToPagedList(paginaNumero, paginaTamanho); 
        }

        public IEnumerable<TesteOptico> UniqueFk()
        {
            return _context.TesteOpticos
               .Select(value => new TesteOptico
               {
                   EstacoesId = value.EstacoesId

               }).ToList();
        }
        public bool EstacaoExiste(string estacao, string sgl)
        {
            return _context.Estacoes.Any(p => p.NomeEstacao == estacao || p.Sigla == sgl);
        }

        public IEnumerable<Estado> Estado()
        {
            return _context.Estados
                .AsNoTracking()
                .AsEnumerable()
                .Select(value => new Estado
                {
                    Id = value.Id,
                    Nome = value.Nome

                }).OrderBy(p => p.Nome)
                .ToList();
        }

        public IEnumerable<Estacoe> Estacao()
        {
            return _context.Estacoes
                .AsEnumerable()
                .Select(value => new Estacoe
                {
                    Id = value.Id,
                    NomeEstacao = value.NomeEstacao,

                }).OrderBy(p => p.NomeEstacao)
                .ToList();
        }

        public IEnumerable<Estacoe> Estacao(string estado)
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
    }
}
