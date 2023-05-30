using ControleGestaoFtth.Models;

namespace ControleGestaoFtth.Repository.Interface
{
    public interface ICdoiaRepository
    {
        Cdoia Cadastrar(Cdoia cdoia);
        Cdoia Atualizar(Cdoia cdoia);
        bool Deletar(int id);
        IEnumerable<Cdoia> Listar(int analiseId);
        Cdoia CarregarId(int id);
    }
}
