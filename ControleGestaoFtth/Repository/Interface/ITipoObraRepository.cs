using ControleGestaoFtth.Models;

namespace ControleGestaoFtth.Repository.Interface
{
    public interface ITipoObraRepository
    {
        TipoObra Cadastrar(TipoObra tipoObra);
        TipoObra Atualizar(TipoObra tipoObra);
        bool Deletar(int id);
        bool TipoObraExiste(string nome);
        IEnumerable<TipoObra> Listar();
        IEnumerable<TipoObra> Listar(int? pagina, string nome);
        IEnumerable<TipoObra> Obras();
        IEnumerable<TesteOptico> UniqueFk();
        TipoObra CarregarId(int id);
    }
}
