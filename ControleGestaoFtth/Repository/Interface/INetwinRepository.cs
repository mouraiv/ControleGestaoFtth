using ControleGestaoFtth.Models;

namespace ControleGestaoFtth.Repository.Interface
{
    public interface INetwinRepository
    {
        Netwin Cadastrar(Netwin netwin);
        Netwin Atualizar(Netwin netwin);
        bool Deletar(int id);
        bool NetwinExiste(int? codigo, string? tipo, string? descricao);
        IEnumerable<Netwin> Listar();
        IEnumerable<Netwin> Listar(int? pagina, string descricao);
        IEnumerable<Netwin> Descricao();
        IEnumerable<TesteOptico> UniqueFk();
        Netwin CarregarId(int id);
    }
}
