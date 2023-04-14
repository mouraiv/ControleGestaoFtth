using ControleGestaoFtth.Context;
using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace ControleGestaoFtth.Repository
{
    public class NetwinRepository : INetwinRepository
    {
        private readonly AppDbContext _context;
        public NetwinRepository(AppDbContext context)
        {
            _context = context;
        }
        public Netwin Atualizar(Netwin netwin)
        {
            Netwin db = CarregarId(netwin.Id);

            if (db == null) throw new Exception("Houve um erro na atualização");

            db.Codigo = netwin.Codigo;
            db.Tipo = netwin.Tipo;
            db.Descricao = netwin.Descricao;

            _context.Netwins.Update(db);
            _context.SaveChanges();

            return db;
        }

        public Netwin Cadastrar(Netwin netwin)
        {
            _context.Netwins.Add(netwin);
            _context.SaveChanges();
            return netwin;
        }

        public Netwin CarregarId(int id)
        {
            return _context.Netwins
                    .Where(p => p.Id == id)
                    .First();
        }

        public bool Deletar(int id)
        {
            Netwin db = CarregarId(id);

            if (db == null) throw new Exception("Houve um erro ao apagar");

            _context.Netwins.Remove(db);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<Netwin> Listar()
        {
            return _context.Netwins
                .AsNoTracking()
                .Select(value => new Netwin
                {
                    Id = value.Id,
                    Codigo = value.Codigo,
                    Tipo = value.Tipo,
                    Descricao = value.Descricao,

                }).OrderBy(p => p.Codigo)
                .ToList();
        }

        public IEnumerable<Netwin> Listar(int? pagina, string descricao)
        {
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);

            IQueryable<Netwin> resultado = _context.Netwins.AsNoTracking();

            if (descricao != null)
            {
                return resultado.
                    Where(p => p.Descricao.Equals(descricao))
                   .ToList().ToPagedList(paginaNumero, paginaTamanho);
            }

            return resultado
                .ToList().ToPagedList(paginaNumero, paginaTamanho);
        }

        public IEnumerable<Netwin> Descricao()
        {
            return _context.Netwins
                .AsNoTracking()
                .AsEnumerable()
                .Select(value => new Netwin
                {
                    Id = value.Id,
                    Descricao = value.Descricao

                }).DistinctBy(p => p.Descricao)
                .OrderBy(p => p.Descricao)
                .ToList();
        }

        public IEnumerable<TesteOptico> UniqueFk()
        {
            return _context.TesteOpticos
               .AsNoTracking()
               .Select(value => new TesteOptico
               {
                   NetwinId = value.NetwinId

               }).ToList();
        }

        public bool NetwinExiste(int? codigo, string? tipo, string? descricao)
        {
            return _context.Netwins.Any(p => p.Codigo == codigo || p.Tipo == tipo || p.Descricao == descricao);
        }
    }
}