using ControleGestaoFtth.Models;

namespace ControleGestaoFtth.Repository.Interface
{
    public interface IEstacoeRepository
    {
        Estacoe Cadastrar(Estacoe construtora);
        Estacoe Atualizar(Estacoe construtora);
        bool Deletar(int id);
        IEnumerable<Estacoe> Listar();
        IEnumerable<Estacoe> Listar(int? pagina, string nomeEstacao, string responsavel);
        Estacoe CarregarId(int id);
    }
}
