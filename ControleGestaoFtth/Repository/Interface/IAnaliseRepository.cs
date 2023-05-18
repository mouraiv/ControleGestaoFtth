using ControleGestaoFtth.Models;

namespace ControleGestaoFtth.Repository.Interface
{
    public interface IAnaliseRepository
    {
        Analise Cadastrar(Analise analise);
        Analise Atualizar(Analise analise);
        bool Deletar(int id);
        Analise CarregarId(int id);
        IEnumerable<Analise> Listar(int? pagina, string? regiao, string estado, string? estacao, int? cdo, int? tecnico, int? status);
        IEnumerable<Analise> UniqueCdo();
        IEnumerable<Tecnico> Tecnico();
        IEnumerable<Regioe> Regiao();
        IEnumerable<Estado> Estado();
        IEnumerable<Estado> Estado(string regiao);
        IEnumerable<Estacoe> Estacoes(string estado);
        IEnumerable<TesteOptico> TesteOptico(string estacao);
    }
}
