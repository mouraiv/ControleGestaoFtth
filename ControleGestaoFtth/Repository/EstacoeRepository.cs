using ControleGestaoFtth.Context;
using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.EntityFrameworkCore;
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
            return _context.Estacoes
                    .Where(p => p.Id == id)
                    .First();
        }

        public bool Deletar(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Estacoe> Listar()
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

        public IEnumerable<Estacoe> Listar(int? pagina, string nomeEstacao, string responsavel)
        {
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);

            IQueryable<Estacoe> resultado = _context.Estacoes.AsNoTracking();

            if (nomeEstacao != null || responsavel != null)
            {
                return resultado.
                    Where(p => p.NomeEstacao.Equals(nomeEstacao) || p.Responsavel.Equals(responsavel))
                   .ToList().ToPagedList(paginaNumero, paginaTamanho);
            }
            
            return resultado
                .ToList().ToPagedList(paginaNumero, paginaTamanho); 
        }

        public IEnumerable<Estacoe> Responsavel()
        {
            return _context.Estacoes
                .AsNoTracking()
                .AsEnumerable()
                .Select(value => new Estacoe
                {
                    Id = value.Id,
                    Responsavel = value.Responsavel

                }).DistinctBy(p => p.Responsavel)
                .OrderBy(p => p.Responsavel)
                .ToList();
        }
    }
}
