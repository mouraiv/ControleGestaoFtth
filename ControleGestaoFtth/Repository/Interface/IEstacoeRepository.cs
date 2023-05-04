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
        IEnumerable<Estacoe> Listar(int? pagina, string estado, string estacao);
        IEnumerable<TesteOptico> UniqueFk();
        IEnumerable<Estado> Estado();
        IEnumerable<Estacoe> Estacao();
        IEnumerable<Estacoe> Estacao(string estado);
        Estacoe CarregarId(int id);
    }
}
