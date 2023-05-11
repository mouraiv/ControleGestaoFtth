using ControleGestaoFtth.Models;

namespace ControleGestaoFtth.Repository.Interface
{
    public interface ITesteOpticoRepository
    {
        int LastId();
        TesteOptico Cadastrar(TesteOptico testeOptico);
        TesteOptico Atualizar(TesteOptico testeOptico);
        bool Deletar(int id);
        TesteOptico CarregarId(int id);
        IEnumerable<TesteOptico> Listar(int? pagina, string? regiao, string estado, string? estacao, string cdo, int? cabo, int? celula);
        IEnumerable<TesteOptico> UniqueCdo();
        IEnumerable<Estacoe> Estacoes();
        IEnumerable<Construtora> Construtoras();
        IEnumerable<Netwin> Netwins();
        IEnumerable<TipoObra> TipoObras();
        IEnumerable<EstadoCampo> EstadoCampos();
        Enderecostotais Enderecototais(string cdo, string municipio, string uf);
        Materiais Materiais(string cdo, string municipio, string uf);
        List<string> ArquivoOptico(string sgl, string cdo, string[] extensoes);
        string GetArquivo(string caminho);
        IEnumerable<Regioe> Regiao();
        IEnumerable<Estado> Estado();
        IEnumerable<Estado> Estado(string reigao);
        IEnumerable<Estacoe> Estacoes(string estado);
        IEnumerable<TesteOptico> FilterCdo(string estacao);
        IEnumerable<TesteOptico> FilterCabo(string estacao);
        IEnumerable<TesteOptico> FilterCelula(string estacao);
    }
}
