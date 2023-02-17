using ControleGestaoFtth.Models;

namespace ControleGestaoFtth.Repository.Interface
{
    public interface ICdoRepository
    {
        Cdo Cadastrar(Cdo cdo);
        Cdo Atualizar(Cdo cdo);
        bool Deletar(int id);
        Cdo CarregarId(int id);
        IEnumerable<Cdo> Listar(int? pagina, string estacao, string cdo, int? cabo, int? celula);
        IEnumerable<Cdo> UniqueCdo();
        IEnumerable<Estacoe> Estacoes();
        IEnumerable<TipoObra> TipoObras();
        IEnumerable<Cdo> FilterCdo(string estacao);
        IEnumerable<Cdo> FilterCabo(string estacao);
        IEnumerable<Cdo> FilterCelula(string estacao);
    }
}
