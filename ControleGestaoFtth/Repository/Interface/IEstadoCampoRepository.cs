using ControleGestaoFtth.Models;

namespace ControleGestaoFtth.Repository.Interface
{
    public interface IEstadoCampoRepository
    {
        EstadoCampo Cadastrar(EstadoCampo estadoCampo);
        EstadoCampo Atualizar(EstadoCampo estadoCampo);
        bool Deletar(int id);
        IEnumerable<EstadoCampo> Listar();
        IEnumerable<EstadoCampo> Listar(int? pagina, string nome);
        IEnumerable<EstadoCampo> Campo();
        IEnumerable<TesteOptico> UniqueFk();
        EstadoCampo CarregarId(int id);
    }
}
