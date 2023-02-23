using ControleGestaoFtth.Models;

namespace ControleGestaoFtth.Repository.Interface
{
    public interface IEstacoeRepository
    {
        Estacoe Cadastrar(Estacoe estacao);
        Estacoe Atualizar(Estacoe estacao);
        bool Deletar(int id);
        IEnumerable<Estacoe> Listar();
        IEnumerable<Estacoe> Listar(int? pagina, string nomeEstacao, string responsavel);
        IEnumerable<Estacoe> Responsavel();
        IEnumerable<TesteOptico> UniqueFk();
        Estacoe CarregarId(int id);
    }
}
