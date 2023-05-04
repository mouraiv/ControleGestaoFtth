using ControleGestaoFtth.Models;

namespace ControleGestaoFtth.Repository.Interface
{
    public interface IEstacoeRepository
    {
        Estacoe Cadastrar(Estacoe estacao);
        Estacoe Atualizar(Estacoe estacao);
        bool Deletar(int id);
        bool EstacaoExiste(string estacao, string sgl);
        IEnumerable<Estacoe> Listar();
        IEnumerable<Estacoe> Listar(int? pagina, string nomeEstacao, string responsavel);
        IEnumerable<TesteOptico> UniqueFk();
        IEnumerable<Estado> Estados();
        Estacoe CarregarId(int id);
    }
}
