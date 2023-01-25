using ControleGestaoFtth.Models;

namespace ControleGestaoFtth.Repository.Interface
{
    public interface IConstrutoraRepository
    {
        Construtora Cadastrar(Construtora construtora);
        Construtora Atualizar(Construtora construtora);
        bool Deletar(int id);
        Construtora CarregarId(int id);
        IEnumerable<Construtora> Listar(int? pagina, string estacao, string cdo, int? cabo, int? celula);
        IEnumerable<Estacoe> Estacoes();
        IEnumerable<Netwin> Netwins();
        IEnumerable<TipoObra> TipoObras();
        IEnumerable<Construtora> FilterCdo(string estacao);
        IEnumerable<Construtora> FilterCabo(string estacao);
        IEnumerable<Construtora> FilterCelula(string estacao);
    }
}
