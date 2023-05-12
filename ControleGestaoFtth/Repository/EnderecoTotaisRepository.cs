using ControleGestaoFtth.Context;
using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;

namespace ControleGestaoFtth.Repository
{
    public class EnderecoTotaisRepository : IEnderecoTotaisRepository
    {
        private readonly AppDbContext _context;
        public EnderecoTotaisRepository(AppDbContext context)
        {
            _context = context;
        }

        public Enderecostotais Atualizar(Enderecostotais enderecostotais)
        {
            Enderecostotais db = CarregarId(enderecostotais.Id);

            if (db == null) throw new Exception("Houve um erro na atualização");

            

            _context.Enderecostotais.Update(db);
            _context.SaveChanges();

            return db;
        }

        public Enderecostotais Cadastrar(Enderecostotais enderecostotais)
        {
            _context.Enderecostotais.Add(enderecostotais);
            _context.SaveChanges();
            return enderecostotais;
        }

        public Enderecostotais CarregarId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Estacoe> Estacoes(string estado)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Estado> Estado()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Estado> Estado(string reigao)
        {
            throw new NotImplementedException();
        }

        public int LastId()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Enderecostotais> Listar()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Enderecostotais> Listar(int? pagina, string? regiao, string estado, string? estacao, string cdo, int codViabilidade)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Netwin> Netwins()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Regioe> Regiao()
        {
            throw new NotImplementedException();
        }
    }
}
