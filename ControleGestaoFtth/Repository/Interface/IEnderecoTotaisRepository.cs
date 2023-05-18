using ControleGestaoFtth.Models;

namespace ControleGestaoFtth.Repository.Interface
{
    public interface IEnderecoTotaisRepository
    {
        int LastId();
        Enderecostotais Cadastrar(Enderecostotais enderecostotais);
        Enderecostotais Atualizar(Enderecostotais enderecostotais);
        Enderecostotais CarregarId(int id);
        IEnumerable<Netwin> Netwins();
        IEnumerable<Regioe> Regiao();
        IEnumerable<Estado> Estado();
        IEnumerable<Estado> Estado(string reigao);
        IEnumerable<Estacoe> Estacoes(string estado);
        IEnumerable<Enderecostotais> Listar();
        IEnumerable<Enderecostotais> Listar(int? pagina, string? regiao, string estado, string? estacao, string cdo, int codViabilidade);
        bool EnderecoTotaisExiste(int idEndereco);
    }
}
