using ControleGestaoFtth.Models;

namespace ControleGestaoFtth.Repository.Interface
{
    public interface INetwinRepository
    {
        Netwin Cadastrar(Netwin netwin);
        Netwin Atualizar(Netwin netwin);
        bool Deletar(int id);
        IEnumerable<Netwin> Listar();
        IEnumerable<Netwin> Listar(int? pagina, string descricao);
        IEnumerable<Netwin> Descricao();
        IEnumerable<Construtora> UniqueFk();
        Netwin CarregarId(int id);
    }
}
