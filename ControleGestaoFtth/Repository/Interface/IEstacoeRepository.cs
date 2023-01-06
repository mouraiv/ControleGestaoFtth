using ControleGestaoFtth.Models;

namespace ControleGestaoFtth.Repository.Interface
{
    public interface IEstacoeRepository
    {
        Estacoe Cadastrar(Estacoe construtora);
        Estacoe Atualizar(Estacoe construtora);
        bool Deletar(int id);
        IEnumerable<Estacoe> Listar();
        Estacoe CarregarId(int id);
    }
}
