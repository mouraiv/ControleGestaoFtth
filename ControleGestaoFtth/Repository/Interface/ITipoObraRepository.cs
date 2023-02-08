using ControleGestaoFtth.Models;

namespace ControleGestaoFtth.Repository.Interface
{
    public interface ITipoObraRepository
    {
        TipoObra Cadastrar(TipoObra tipoObra);
        TipoObra Atualizar(TipoObra tipoObra);
        bool Deletar(int id);
        IEnumerable<TipoObra> Listar();
        IEnumerable<TipoObra> Listar(int? pagina, string nome);
        IEnumerable<TipoObra> Obras();
        IEnumerable<Construtora> UniqueFk();
        TipoObra CarregarId(int id);
    }
}
