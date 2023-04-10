using ControleGestaoFtth.ComponentModel;
using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Text;

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
            _progressBar.Progresso = 0;

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

            int id = 0;
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
            int id = 0;
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
            int id = 0;
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
            int id = 0;
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
            var viabilidade = _TesteOpticoRepository.Enderecototais(cdo, municipio);

            int? codViabilidade = -1;
            int id = -1;

            foreach (var codigo in viabilidade)
            {
                codViabilidade = codigo.COD_VIABILIDADE;
                break;
            }
            foreach (var netwin in _TesteOpticoRepository.Netwins())
            {
                if (netwin.Codigo == codViabilidade) {
                    id = netwin.Id;
                    break;
                }
            }
            return id;
        }

        [HttpPost]
        public IActionResult Importar(IFormFile file)
        {
            try
            {
                if (file != null) {
                    // Ler os dados do arquivo XLSX
                    var dados = new List<TesteOptico>();

                    _arquivoModel.ImportarXlsx(file.OpenReadStream());

                    using (var pacote = new ExcelPackage(file.OpenReadStream()))
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        var planilha = pacote.Workbook.Worksheets[0];

                        for (int rows = planilha.Dimension.Start.Row + 7; rows <= planilha.Dimension.End.Row; rows++)
                        {
                            var testeOptico = new TesteOptico();

                            //CONTADOR DE PROGRESSO IMPORTAÇÃO
                            _progressBar.Progresso = rows * 100 / _arquivoModel.TamanhoTotalXlsx;

                            if (planilha.Cells[rows, 2].Value != null)
                            {
                                testeOptico.ConstrutorasId = GetForeignKeyConstrutora(planilha.Cells[rows, 2].Value.ToString()?.ToUpper() ?? "");
                            }
                            if (planilha.Cells[rows, 3].Value != null)
                            {
                                testeOptico.EstacoesId = GetForeignKeyEstacao(planilha.Cells[rows, 3].Value.ToString()?.ToUpper() ?? "");
                            }
                            if (planilha.Cells[rows, 4].Value != null)
                            {
                                testeOptico.TipoObraId = GetForeignKeyTipoObra(planilha.Cells[rows, 4].Value.ToString()?.ToUpper() ?? "");
                            }
                            if (planilha.Cells[rows, 5].Value != null)
                            {
                                testeOptico.Cabo = int.Parse(planilha.Cells[rows, 5].Value.ToString() ?? "");
                            }
                            if (planilha.Cells[rows, 6].Value != null)
                            {
                                testeOptico.Celula = int.Parse(planilha.Cells[rows, 6].Value.ToString() ?? "");
                            }
                            if (planilha.Cells[rows, 7].Value != null)
                            {
                                testeOptico.CDO = planilha.Cells[rows, 7].Value.ToString() ?? "";
                                testeOptico.NetwinId = GetCodViabilidadeEnderecosTotais(planilha.Cells[rows, 7].Value.ToString()?.ToUpper() ?? "", planilha.Cells[rows, 3].Value.ToString()?.ToUpper() ?? "");
                            }
                            if (planilha.Cells[rows, 8].Value != null)
                            {
                                testeOptico.Capacidade = int.Parse(planilha.Cells[rows, 8].Value.ToString() ?? "");
                            }
                            if (planilha.Cells[rows, 9].Value != null)
                            {
                                testeOptico.TotalUms = int.Parse(planilha.Cells[rows, 9].Value.ToString() ?? "");
                            }
                            if (planilha.Cells[rows, 10].Value != null)
                            {
                                testeOptico.EstadoCamposId = GetForeignKeyEstadoCampo(planilha.Cells[rows, 10].Value.ToString()?.ToUpper() ?? "");
                            }
                            if (planilha.Cells[rows, 11].Value != null)
                            {
                                testeOptico.DatadeConstrucao = DateTime.FromOADate(double.Parse(planilha.Cells[rows, 11].Value.ToString() ?? ""));
                            }
                            if (planilha.Cells[rows, 13].Value != null)
                            {
                                testeOptico.EquipedeConstrucao = planilha.Cells[rows, 13].Value.ToString() ?? "";
                            }
                            if (planilha.Cells[rows, 14].Value != null)
                            {
                                testeOptico.DatadoTeste = DateTime.FromOADate(double.Parse(planilha.Cells[rows, 14].Value.ToString() ?? ""));
                            }
                            if (planilha.Cells[rows, 15].Value != null)
                            {
                                testeOptico.DatadeRecebimento = DateTime.FromOADate(double.Parse(planilha.Cells[rows, 15].Value.ToString() ?? ""));
                            }
                            if (planilha.Cells[rows, 17].Value != null)
                            {
                                testeOptico.Tecnico = planilha.Cells[rows, 17].Value.ToString();
                            }
                            if (planilha.Cells[rows, 18].Value != null)
                            {
                                testeOptico.PosicaoICX_DGO = planilha.Cells[rows, 18].Value.ToString();
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
                    }

                    // Salvar os dados no banco de dados
                    foreach (var optico in dados)
                    {
                       _TesteOpticoRepository.Cadastrar(optico);
                    }
                    TempData["Sucesso"] = "Inportação concluída.";
                    // Redirecionar o usuário de volta para a página inicial
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Falha"] = $"Arquivo de importação vazio";
                    // Redirecionar o usuário de volta para a página inicial
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                TempData["Falha"] = $"Erro na importação {ex.Message}";
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
                TesteOptico TesteOptico = _TesteOpticoRepository.CarregarId(id);

                return View(TesteOptico);
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
