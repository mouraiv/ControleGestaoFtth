using ControleGestaoFtth.Models;

namespace ControleGestaoFtth.Repository.Interface
{
    public interface IConstrutoraRepository
    {
        Construtora Cadastrar(Construtora construtora);
        Construtora Atualizar(Construtora construtora);
        bool Deletar(int id);
        bool ContrutoraExiste(string nome);
        IEnumerable<Construtora> Listar();
        IEnumerable<Construtora> Listar(int? pagina, string nome);
        IEnumerable<Construtora> Obras();
        IEnumerable<TesteOptico> UniqueFk();
        Construtora CarregarId(int id);
    }
}
