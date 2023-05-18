using ControleGestaoFtth.ComponentModel;
using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;

namespace ControleGestaoFtth.Controllers
{
    public class EnderecoTotaisController : Controller
    {
        private readonly IEnderecoTotaisRepository _enderecoTotaisRepository;
        private readonly ProgressBar _progressBar;

        public EnderecoTotaisController(IEnderecoTotaisRepository enderecoTotaisRepository, ProgressBar progressBar)
        {
            _enderecoTotaisRepository = enderecoTotaisRepository;
            _progressBar = progressBar;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Importar()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> Importar(IFormFile file)
        {
            try
            {
                //if (file != null && file.Length > 0)
                //{
                    // Lê o arquivo CSV
                    var reader = new StreamReader(file.OpenReadStream());
                    var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                    var records = csv.GetRecords<Enderecostotais>().ToList();
                    int contador = 0;

                    await Task.Run(() =>
                    {
                        // Grava os dados no banco de dados
                        foreach (var enderecostotais in records)
                        {
                            //CARREGAR PROGRESSO
                            _progressBar.Progresso = contador++;

                            var entity = new Enderecostotais
                            {
                                //CARREGAR PROPRIEDADES A SEREM IMPORTADAS
                                //Id = enderecostotais.Id,
                                CELULA = enderecostotais.CELULA,
                                ESTACAO_ABASTECEDORA = enderecostotais.ESTACAO_ABASTECEDORA,
                                UF = enderecostotais.UF,
                                MUNICIPIO = enderecostotais.MUNICIPIO,
                                LOCALIDADE = enderecostotais.LOCALIDADE,
                                COD_LOCALIDADE = enderecostotais.COD_LOCALIDADE,
                                LOCALIDADE_ABREV = enderecostotais.LOCALIDADE_ABREV,
                                LOGRADOURO = enderecostotais.LOGRADOURO,
                                COD_LOGRADOURO = enderecostotais.COD_LOGRADOURO,
                                NUM_FACHADA = enderecostotais.NUM_FACHADA,
                                COMPLEMENTO = enderecostotais.COMPLEMENTO,
                                COMPLEMENTO2 = enderecostotais.COMPLEMENTO2,
                                COMPLEMENTO3 = enderecostotais.COMPLEMENTO3,
                                CEP = enderecostotais.CEP,
                                BAIRRO = enderecostotais.BAIRRO,
                                COD_SURVEY = enderecostotais.COD_SURVEY,
                                QUANTIDADE_UMS = enderecostotais.QUANTIDADE_UMS,
                                COD_VIABILIDADE = enderecostotais.COD_VIABILIDADE,
                                TIPO_VIABILIDADE = enderecostotais.TIPO_VIABILIDADE,
                                TIPO_REDE = enderecostotais.TIPO_REDE,
                                UCS_RESIDENCIAIS = enderecostotais.UCS_RESIDENCIAIS,
                                UCS_COMERCIAIS = enderecostotais.UCS_COMERCIAIS,
                                NOME_CDO = enderecostotais.NOME_CDO,
                                ID_ENDERECO = enderecostotais.ID_ENDERECO,
                                LATITUDE = enderecostotais.LATITUDE,
                                LONGITUDE = enderecostotais.LONGITUDE,
                                TIPO_SURVEY = enderecostotais.TIPO_SURVEY,
                                REDE_INTERNA = enderecostotais.REDE_INTERNA,
                                UMS_CERTIFICADAS = enderecostotais.UMS_CERTIFICADAS,
                                REDE_EDIF_CERT = enderecostotais.REDE_EDIF_CERT,
                                NUM_PISOS = enderecostotais.NUM_PISOS,
                                DISP_COMERCIAL = enderecostotais.DISP_COMERCIAL,
                                ESTADO_CONTROLE = enderecostotais.ESTADO_CONTROLE,
                                DATA_ESTADO_CONTROLE = enderecostotais.DATA_ESTADO_CONTROLE,
                                ID_CELULA = enderecostotais.ID_CELULA,
                                QUANTIDADE_HCS = enderecostotais.QUANTIDADE_HCS,
                                PROJETO = enderecostotais.PROJETO,
                            };

                            //VERIFICA SE O REGISTRO DA CSV EXISTE NO BANCO DE DADOS
                            if (_enderecoTotaisRepository.EnderecoTotaisExiste(entity.ID_ENDERECO))
                            {
                                //SE EXISTIR, ATUALIZAR
                                //_enderecoTotaisRepository.Atualizar(entity);
                                Debug.WriteLine("----------------- ATUALIZAR -----------------");
                            }
                            else
                            {
                                //SE NÃO EXISTIR, CADASTRAR
                                //_enderecoTotaisRepository.Cadastrar(entity);
                                Debug.WriteLine("----------------- CADASTRAR -----------------");
                            }

                        };
                    });

                    //ALETA DE SUCESSO
                    TempData["Sucesso"] = "CSV [Endereço Totais] importado com sucesso.";
                    //RESETAR BARRA DE PROGRESSO SESSÃO IMPORT
                    _progressBar.Progresso = 0;
                    // Redirecionar o usuário de volta para a página inicial
                    return RedirectToAction("Index");

                //}
                /*else
                {
                    //ALERTA DE FALHA
                    TempData["Falha"] = "O Arquivo CSV está vazio.";
                    //RESETAR BARRA DE PROGRESSO SESSÃO IMPORT
                    _progressBar.Progresso = 0;
                    // Redirecionar o usuário de volta para a página inicial
                    return RedirectToAction("Index");
                }*/
            }
            catch (Exception ex)
            {
                //ALERTA DE FALHA
                TempData["Falha"] = $"ATENÇÃO! {ex.Message}";
                //RESETAR BARRA DE PROGRESSO SESSÃO IMPORT

                // Redirecionar o usuário de volta para a página inicial
                return RedirectToAction("Index");
            }
        }
    }
}
