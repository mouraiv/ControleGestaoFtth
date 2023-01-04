using ControleGestaoFtth.Models;

namespace ControleGestaoFtth.Repository.Interface
{
    public interface IConstrutoraRepository
    {
        Construtora Cadastrar(Construtora construtora);
        Construtora Atualizar(Construtora construtora);
        bool Deletar(int id);
        //Listar Base FTTH
        IEnumerable<Construtora> Listar(int? pagina);
        Construtora CarregarId(int id);
        //Listar Base FTTH
        Task<IEnumerable<Construtora>> Listar();
    }
}
