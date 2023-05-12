using ControleGestaoFtth.Context;
using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;

namespace ControleGestaoFtth.Repository
{
    public class EnderecoTotaisRepository : IEnderecoTotaisRepository
    {
        private readonly AppDbContext _context;
        public EnderecoTotaisRepository(AppDbContext context)
        {
            _context = context;
        }

        public Enderecostotais Atualizar(Enderecostotais enderecostotais)
        {
            Enderecostotais db = CarregarId(enderecostotais.Id);

            if (db == null) throw new Exception("Houve um erro na atualização");

            //CAMPOS A SEREM ATUALIZADOS
            db.Id = enderecostotais.Id;
            db.CELULA = enderecostotais.CELULA;
            db.ESTACAO_ABASTECEDORA = enderecostotais.ESTACAO_ABASTECEDORA; 
            db.UF = enderecostotais.UF;
            db.MUNICIPIO = enderecostotais.MUNICIPIO;
            db.LOCALIDADE = enderecostotais.LOCALIDADE;
            db.COD_LOCALIDADE = enderecostotais.COD_LOCALIDADE;
            db.LOCALIDADE_ABREV = enderecostotais.LOCALIDADE_ABREV;
            db.LOGRADOURO = enderecostotais.LOGRADOURO;
            db.COD_LOGRADOURO = enderecostotais.COD_LOGRADOURO;
            db.NUM_FACHADA = enderecostotais.NUM_FACHADA; 
            db.COMPLEMENTO = enderecostotais.COMPLEMENTO;
            db.COMPLEMENTO2 = enderecostotais.COMPLEMENTO2;
            db.COMPLEMENTO3 = enderecostotais.COMPLEMENTO3;
            db.CEP = enderecostotais.CEP;
            db.BAIRRO = enderecostotais.BAIRRO;
            db.COD_SURVEY = enderecostotais.COD_SURVEY;
            db.QUANTIDADE_UMS = enderecostotais.QUANTIDADE_UMS;
            db.COD_VIABILIDADE = enderecostotais.COD_VIABILIDADE;
            db.TIPO_VIABILIDADE = enderecostotais.TIPO_VIABILIDADE;
            db.TIPO_REDE = enderecostotais.TIPO_REDE;
            db.UCS_RESIDENCIAIS = enderecostotais.UCS_RESIDENCIAIS;
            db.UCS_COMERCIAIS = enderecostotais.UCS_COMERCIAIS;
            db.NOME_CDO = enderecostotais.NOME_CDO;
            db.ID_ENDERECO = enderecostotais.ID_ENDERECO;
            db.LATITUDE = enderecostotais.LATITUDE;
            db.LONGITUDE = enderecostotais.LONGITUDE;
            db.TIPO_SURVEY = enderecostotais.TIPO_SURVEY;
            db.REDE_INTERNA = enderecostotais.REDE_INTERNA;
            db.UMS_CERTIFICADAS = enderecostotais.UMS_CERTIFICADAS;
            db.REDE_EDIF_CERT = enderecostotais.REDE_EDIF_CERT;
            db.NUM_PISOS = enderecostotais.NUM_PISOS;
            db.DISP_COMERCIAL = enderecostotais.DISP_COMERCIAL;
            db.ESTADO_CONTROLE = enderecostotais.ESTADO_CONTROLE;
            db.DATA_ESTADO_CONTROLE = enderecostotais.DATA_ESTADO_CONTROLE;
            db.ID_CELULA = enderecostotais.ID_CELULA;
            db.QUANTIDADE_HCS = enderecostotais.QUANTIDADE_HCS;
            db.PROJETO = enderecostotais.PROJETO;

            _context.Enderecostotais.Update(db);
            _context.SaveChanges();

            return db;
        }

        public Enderecostotais Cadastrar(Enderecostotais enderecostotais)
        {
            _context.Enderecostotais.Add(enderecostotais);
            _context.SaveChanges();
            return enderecostotais;
        }

        public Enderecostotais CarregarId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Estacoe> Estacoes(string estado)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Estado> Estado()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Estado> Estado(string reigao)
        {
            throw new NotImplementedException();
        }

        public int LastId()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Enderecostotais> Listar()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Enderecostotais> Listar(int? pagina, string? regiao, string estado, string? estacao, string cdo, int codViabilidade)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Netwin> Netwins()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Regioe> Regiao()
        {
            throw new NotImplementedException();
        }
    }
}
