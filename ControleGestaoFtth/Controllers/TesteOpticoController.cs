using ControleGestaoFtth.ComponentModel;
using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Diagnostics;

namespace ControleGestaoFtth.Controllers
{
    public class TesteOpticoController : Controller
    {
        private readonly ITesteOpticoRepository _TesteOpticoRepository;
        private readonly ConversionViewModel _conversionViewModel;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ProgressBar _progressBar;
        private readonly ArquivoModel _arquivoModel;
        public TesteOpticoController(ITesteOpticoRepository TesteOpticoRepository, ArquivoModel arquivoModel, ConversionViewModel conversionViewModel, ProgressBar progressBar, IWebHostEnvironment webHostEnvironment)
        {
            _TesteOpticoRepository = TesteOpticoRepository;
            _conversionViewModel = conversionViewModel;
            _webHostEnvironment = webHostEnvironment;
            _arquivoModel = arquivoModel;
            _progressBar = progressBar;
        }
        public IActionResult Index()
        {
            ViewData["selectEstacao"] = _TesteOpticoRepository.Estacoes();
            if (TempData["HstEstacao"] != null)
            {
                var estacao = TempData["HstEstacao"] as string;
                ViewBag.HistoricoEstacao = estacao;
            }
             
            return View();
        }
        public ActionResult Arquivo()
        {

            var listarArquivos = new List<ArquivoModel>();

            string diretorios = _TesteOpticoRepository.GetArquivo("~\\..\\Download\\");

            foreach (string arq in Directory.GetFiles(diretorios))
            {
                listarArquivos.Add(new ArquivoModel
                {
                    Nome = Path.GetFileName(arq),
                    Caminho = arq
                });
            }

            return PartialView(listarArquivos);
        }
        public ActionResult Download(string caminho)
        {
            string file = Path.Combine(_webHostEnvironment.ContentRootPath, caminho.Replace("~\\..\\", ""));

            MemoryStream output = new();

            using (var stream = new FileStream(file, FileMode.Open))
            {
                stream.CopyTo(output);
            }
            return File(output.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.GetFileName(caminho));
        }
        public IActionResult Importar()
        {
            return PartialView();
        }
        public int GetForeignKeyConstrutora(string value)
        {

            int id = 6;
            foreach (var construtora in _TesteOpticoRepository.Construtoras())
            {
                if (construtora.Nome.Equals(value))
                {
                    id = construtora.Id;
                    break;
                }
            }
            return id;
        }
        public int GetForeignKeyEstacao(string value)
        {
            int id = 100;
            foreach (var estacao in _TesteOpticoRepository.Estacoes())
            {
                if (estacao.NomeEstacao.Equals(value))
                {
                    id = estacao.Id; 
                    break;
                }
            }
            return id;
        }
        public int GetForeignKeyTipoObra(string value)
        {
            int id = 61;
            foreach (var tipoObra in _TesteOpticoRepository.TipoObras())
            {
                if (tipoObra.Nome.Equals(value))
                {
                    id = tipoObra.Id;
                    break;
                }
            }
            return id;
        }
        public int GetForeignKeyEstadoCampo(string value)
        {
            int id = 44;
            foreach (var estadoCampo in _TesteOpticoRepository.EstadoCampos())
            {
                if (estadoCampo.Nome.Equals(value))
                {
                    id = estadoCampo.Id;
                    break;
                }
            }
            return id;
        }

        public int GetCodViabilidadeEnderecosTotais(string cdo, string municipio)
        {
            try
            {
                int? codViabilidade = _TesteOpticoRepository.Enderecototais(cdo, municipio).COD_VIABILIDADE;
                int id = 0;

                foreach (var netwin in _TesteOpticoRepository.Netwins())
                {
                    if (netwin.Codigo == codViabilidade)
                    {
                        id = netwin.Id;
                        break;
                    }
                }
                return id;
            }
            catch (Exception)
            {
                return 1;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Importar(IFormFile file)
        {
            try
            {
                // Ler os dados do arquivo XLSX
                var dados = new List<TesteOptico>();

                _arquivoModel.ImportarXlsx(file.OpenReadStream());

                using (var pacote = new ExcelPackage(file.OpenReadStream()))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    var planilha = pacote.Workbook.Worksheets[0];

                    int totalRows = _arquivoModel.TamanhoTotalXlsx + 7;

                    if (totalRows < 8)
                    {
                        TempData["Falha"] = $"Arquivo de importação vazio ou não contém dados em suas colunas.";
                        // Redirecionar o usuário de volta para a página inicial
                        return RedirectToAction("Index");

                    }else if (totalRows > 38 )
                    {
                        TempData["Falha"] = $"Arquivo de importação não podem exceder o limite de 30 linhas.";
                        // Redirecionar o usuário de volta para a página inicial
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        await Task.Run(() =>
                        {

                            for (int rows = 8; rows <= totalRows; rows++)
                            {
                                var testeOptico = new TesteOptico();

                                testeOptico.Id = _TesteOpticoRepository.LastId() + (rows - 7);
                                testeOptico.StatesId = 77;

                                if (planilha.Cells[rows, 2].Value != null)
                                {
                                    testeOptico.ConstrutorasId = GetForeignKeyConstrutora(planilha.Cells[rows, 2].Value.ToString() ?? "");
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -Construtora- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 3].Value != null)
                                {
                                    testeOptico.EstacoesId = GetForeignKeyEstacao(planilha.Cells[rows, 3].Value.ToString() ?? "");
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -Estação- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 4].Value != null)
                                {
                                    testeOptico.TipoObraId = GetForeignKeyTipoObra(planilha.Cells[rows, 4].Value.ToString() ?? "");
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -Tipo de Obra- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 5].Value != null)
                                {
                                    testeOptico.Cabo = int.Parse(planilha.Cells[rows, 5].Value.ToString() ?? "");
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -Cabo- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 6].Value != null)
                                {
                                    testeOptico.Celula = int.Parse(planilha.Cells[rows, 6].Value.ToString() ?? "");
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -Célula- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 7].Value != null)
                                {
                                    testeOptico.CDO = planilha.Cells[rows, 7].Value.ToString() ?? "";
                                    testeOptico.NetwinId = GetCodViabilidadeEnderecosTotais(planilha.Cells[rows, 7].Value.ToString() ?? "", planilha.Cells[rows, 3].Value.ToString() ?? "");
                                    //CONTADOR DE PROGRESSO IMPORTAÇÃO
                                    _progressBar.Progresso = rows * 100 / totalRows;
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -CDO- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 8].Value != null)
                                {
                                    testeOptico.Capacidade = int.Parse(planilha.Cells[rows, 8].Value.ToString() ?? "");
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -Capacidade- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 9].Value != null)
                                {
                                    testeOptico.TotalUms = int.Parse(planilha.Cells[rows, 9].Value.ToString() ?? "");
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -Total UMs- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 10].Value != null)
                                {
                                    testeOptico.EstadoCamposId = GetForeignKeyEstadoCampo(planilha.Cells[rows, 10].Value.ToString()?.ToUpper() ?? "");
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -Estado de Campo- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 11].Value != null)
                                {
                                    testeOptico.DatadeConstrucao = DateTime.FromOADate(double.Parse(planilha.Cells[rows, 11].Value.ToString() ?? ""));
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -Data de Construção- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 13].Value != null)
                                {
                                    testeOptico.EquipedeConstrucao = planilha.Cells[rows, 13].Value.ToString() ?? "";
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -Equipe de Contrução- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 14].Value != null)
                                {
                                    testeOptico.DatadoTeste = DateTime.FromOADate(double.Parse(planilha.Cells[rows, 14].Value.ToString() ?? ""));
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -Data do Teste- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 15].Value != null)
                                {
                                    testeOptico.DatadeRecebimento = DateTime.FromOADate(double.Parse(planilha.Cells[rows, 15].Value.ToString() ?? ""));
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -DATA DE ENVIO DO TESTE- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 17].Value != null)
                                {
                                    testeOptico.Tecnico = planilha.Cells[rows, 17].Value.ToString();
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -TÉCNICO DO TESTE- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 18].Value != null)
                                {
                                    testeOptico.PosicaoICX_DGO = planilha.Cells[rows, 18].Value.ToString();
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -POSIÇÃO ICX/DGO- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 19].Value != null)
                                {
                                    testeOptico.FibraDGO = planilha.Cells[rows, 19].Value.ToString();
                                }
                                if (planilha.Cells[rows, 20].Value != null)
                                {
                                    testeOptico.SplitterCEOS = planilha.Cells[rows, 20].Value.ToString();
                                }
                                if (planilha.Cells[rows, 21].Value != null)
                                {
                                    testeOptico.BobinadeLancamento = int.Parse(planilha.Cells[rows, 21].Value.ToString() ?? "");
                                }
                                if (planilha.Cells[rows, 22].Value != null)
                                {
                                    testeOptico.BobinadeRecepcao = int.Parse(planilha.Cells[rows, 22].Value.ToString() ?? "");
                                }
                                if (planilha.Cells[rows, 23].Value != null)
                                {
                                    testeOptico.QuantidadeDeTeste = int.Parse(planilha.Cells[rows, 23].Value.ToString() ?? "");
                                }
                                dados.Add(testeOptico);

                            }
                            // Salvar os dados no banco de dados
                            foreach (var optico in dados)
                            {
                                _TesteOpticoRepository.Cadastrar(optico);
                            }
                        });
                        //ALETA DE SUCESSO
                        TempData["Sucesso"] = "Inportação concluída.";
                        //RESETAR BARRA DE PROGRESSO SESSÃO IMPORT
                        _progressBar.Progresso = 0;
                        // Redirecionar o usuário de volta para a página inicial
                        return RedirectToAction("Index");
                    }
                }
            }
            catch(Exception ex)
            {
                //ALERTA DE FALHA
                TempData["Falha"] = $"ATENÇÃO! {ex.Message}";
                //RESETAR BARRA DE PROGRESSO SESSÃO IMPORT
                _progressBar.Progresso = 0;
                // Redirecionar o usuário de volta para a página inicial
                return RedirectToAction("Index");
            }
        }

        public IActionResult Inserir()
        {
            ViewData["selectViabilidade"] = _TesteOpticoRepository.Netwins();
            ViewData["selectConstrutora"] = _TesteOpticoRepository.Construtoras();
            ViewData["selectEstacao"] = _TesteOpticoRepository.Estacoes();
            ViewData["selectObras"] = _TesteOpticoRepository.TipoObras();
            ViewData["selectEstadoCampo"] = _TesteOpticoRepository.EstadoCampos();

            return View();
        }
        public IActionResult Editar(int id)
        {
            TesteOptico TesteOptico = _TesteOpticoRepository.CarregarId(id);
            ViewData["selectViabilidade"] = _TesteOpticoRepository.Netwins();
            ViewData["selectConstrutora"] = _TesteOpticoRepository.Construtoras();
            ViewData["selectEstacao"] = _TesteOpticoRepository.Estacoes();
            ViewData["selectObras"] = _TesteOpticoRepository.TipoObras();
            ViewData["selectEstadoCampo"] = _TesteOpticoRepository.EstadoCampos();

            return View(TesteOptico);
        }
        public IActionResult Confirmacao(int id)
        {
            TesteOptico TesteOptico = _TesteOpticoRepository.CarregarId(id);

            return View(TesteOptico);
        }
        [HttpGet]
        public IActionResult Listar(int? pagina, string estacao, string cdo, int? cabo, int? celula)
        {
            if (estacao != null)
            {
                ViewData["selectCdoFilter"] = _TesteOpticoRepository.FilterCdo(estacao);
                ViewData["selectCaboFilter"] = _TesteOpticoRepository.FilterCabo(estacao);
                ViewData["selectCelulaFilter"] = _TesteOpticoRepository.FilterCelula(estacao);
            }
            else
            {
                ViewData["selectCdoFilter"] = _TesteOpticoRepository.FilterCdo("");
                ViewData["selectCaboFilter"] = _TesteOpticoRepository.FilterCabo("");
                ViewData["selectCelulaFilter"] = _TesteOpticoRepository.FilterCelula("");
            }

            try
            {
                IEnumerable<TesteOptico> listar = _TesteOpticoRepository.Listar(pagina, estacao ?? "", cdo, cabo, celula);

                return PartialView(listar);
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao listar - {error.Message}.";
                return PartialView();
            }

        }
        [HttpGet]
        public IActionResult Detalhe(int id)
        {

            try
            {
                TesteOptico testeOptico = _TesteOpticoRepository.CarregarId(id);

                TempData["HstEstacao"] = testeOptico.Estacao.NomeEstacao.ToString();

                return View(testeOptico);
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao listar - {error.Message}.";

                return RedirectToAction("Index");
            }

        }
        [HttpGet]
        public IActionResult FileResultImg(string sgl, string cdo)
        {
            try
            {
                var imagem = _TesteOpticoRepository.ArquivoOptico(sgl, cdo, new string[] { ".jpg", ".png", ".jfif", ".bmp" });
                ViewData["imagemOptica"] = true;
               
                return Json(new
                {
                    imagemOptica = ViewData["imagemOptica"]
                });
            }
            catch (Exception)
            {
                return new EmptyResult();
            }
        }
        [HttpGet]
        public IActionResult FileResultDwg(string sgl, string cdo)
        {
            try
            {
                var dwg = _TesteOpticoRepository.ArquivoOptico(sgl, cdo, new string[] { ".dwg" });

                if (dwg[0].Length > 0) 
                {
                    ViewData["dwgToPdf"] = true;
                }
          
                return Json(new {
                    dwgToPdf = ViewData["dwgToPdf"]
                });
            }
            catch (Exception)
            {
                return new EmptyResult();
            }

        }
        [HttpGet]
        public IActionResult ImgOptico(string sgl, string cdo)
        {

            try
            {
                var imagem = _TesteOpticoRepository.ArquivoOptico(sgl, cdo, new string[] { ".jpg", ".png", ".jfif", ".bmp" });

                return PartialView(imagem);
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao Carregar Imagem Optica - {error.Message}.";

                return RedirectToAction("Index");
            }

        }
        [HttpGet]
        public IActionResult DwgOptico(string sgl, string cdo)
        {
            try
            {
                var dwg = _TesteOpticoRepository.ArquivoOptico(sgl, cdo, new string[] { ".dwg" });

                _conversionViewModel.InputFilePath = dwg[0];

                _conversionViewModel.ConvertFileInBackground();

                return RedirectToAction("DwgView");

            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao Carregar DWG - {error.Message}.";
                return RedirectToAction("Index");
            }

        }
        [HttpGet]
        public IActionResult DwgView()
        {
            try
            {
                return File(_conversionViewModel.OutputFilePath, "application/pdf");
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao Carregar DWG - {error.Message}.";
                return RedirectToAction("Index");
            }

        }
        [HttpPost]
        public IActionResult Inserir(TesteOptico TesteOptico)
        {
            ViewData["selectViabilidade"] = _TesteOpticoRepository.Netwins();
            ViewData["selectConstrutora"] = _TesteOpticoRepository.Construtoras();
            ViewData["selectEstacao"] = _TesteOpticoRepository.Estacoes();
            ViewData["selectObras"] = _TesteOpticoRepository.TipoObras();
            ViewData["selectEstadoCampo"] = _TesteOpticoRepository.EstadoCampos();

            try
            {
                if (ModelState.IsValid)
                {
                    if (_TesteOpticoRepository.UniqueCdo().Any(p => p.CDO.Equals(TesteOptico.CDO.ToUpper())))
                    {
                        TempData["Falha"] = $"CDO {TesteOptico.CDO} já existe.";

                        return View(TesteOptico);
                    }
                    else
                    {
                        _TesteOpticoRepository.Cadastrar(TesteOptico);
                        TempData["Sucesso"] = "Inserido com sucesso.";
                        return RedirectToAction("Inserir");
                    }
                }
                return View(TesteOptico);
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao inserir - {error.Message}.";
                return View(TesteOptico);
            }
        }
        [HttpPost]
        public IActionResult Editar(TesteOptico TesteOptico)
        {
            try
            {
                _TesteOpticoRepository.Atualizar(TesteOptico);
                TempData["Sucesso"] = "Editado com sucesso.";
                return RedirectToAction("Editar", new { id = TesteOptico.Id });

            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao editar - {error.Message}.";
                return View(TesteOptico);
            }
        }
        [HttpGet]
        public IActionResult Apagar(int id)
        {
            try
            {
                _TesteOpticoRepository.Deletar(id);
                TempData["Sucesso"] = "Teste óptico Excluído com sucesso.";
                return RedirectToAction("Index");
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao excluir - {error.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
