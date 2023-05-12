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

            ViewData["selectRegiao"] = _TesteOpticoRepository.Regiao();
            ViewData["selectEstado"] = _TesteOpticoRepository.Estado();
            ViewData["selectEstacao"] = _TesteOpticoRepository.Estacoes();

            if (TempData["HstEstado"] != null)
            {
                var regiao = TempData["HstRegiao"] as string;
                var estacao = TempData["HstEstacao"] as string;
                var estado = TempData["HstEstado"] as string;
                ViewBag.HistoricoRegiao = regiao;
                ViewBag.HistoricoEstacao = estacao;
                ViewBag.HistoricoEstado = estado;
            }
             
            return View();
        }
        public IActionResult GetListDropDown(string regiao, string estado, string estacao)
        {
            if (regiao != null && estado == null)
            {
                var estadoList = _TesteOpticoRepository.Estado(regiao).Select(value => new Estado { Nome = value.Nome });
                return Json(new { estado = estadoList });

            }
            else if (estado != null)
            {
                var estacaoList = _TesteOpticoRepository.Estacoes(estado).Select(value => new Estacoe { NomeEstacao = value.NomeEstacao });
                return Json(new { estacao = estacaoList });
            }
            else
            {
                return Json(new
                {
                    estado = _TesteOpticoRepository.Estado().Select(value => new Estado { Nome = value.Nome }),
                    estacao = _TesteOpticoRepository.Estacoes().Select(value => new Estacoe { NomeEstacao = value.NomeEstacao })
                });
            }
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
        public int GetForeignKeyEstacao(string value, string estado)
        {
            int id = 100;
            foreach (var estados in _TesteOpticoRepository.Estado())
            {
                foreach (var estacao in _TesteOpticoRepository.Estacoes())
                {
                    if (estacao.NomeEstacao.Equals(value) && estados.Nome == estado)
                    {
                        id = estacao.Id;
                        break;
                    }
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

        public int GetCodViabilidadeEnderecosTotais(string cdo, string municipio, string uf)
        {
            try
            {
                int? codViabilidade = _TesteOpticoRepository.Enderecototais(cdo, municipio, uf).COD_VIABILIDADE;
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
                                    testeOptico.ConstrutorasId = GetForeignKeyConstrutora(planilha.Cells[rows, 2].Value.ToString()?.ToUpper() ?? "");
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -Construtora- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 3].Value == null)
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -Estado (BRASIL)- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 4].Value != null)
                                {
                                    testeOptico.EstacoesId = GetForeignKeyEstacao(planilha.Cells[rows, 4].Value.ToString()?.ToUpper() ?? "", planilha.Cells[rows, 3].Value.ToString()?.ToUpper() ?? "");
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -Estação- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 5].Value != null)
                                {
                                    testeOptico.TipoObraId = GetForeignKeyTipoObra(planilha.Cells[rows, 5].Value.ToString()?.ToUpper() ?? "");
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -Tipo de Obra- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 6].Value != null)
                                {
                                    testeOptico.Cabo = int.Parse(planilha.Cells[rows, 6].Value.ToString()?.ToUpper() ?? "");
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -Cabo- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 7].Value != null)
                                {
                                    testeOptico.Celula = int.Parse(planilha.Cells[rows, 7].Value.ToString()?.ToUpper() ?? "");
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -Célula- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 8].Value != null)
                                {
                                    var dataEstadoOperacional = _TesteOpticoRepository.Materiais(planilha.Cells[rows, 8].Value.ToString()?.ToUpper() ?? "", planilha.Cells[rows, 4].Value.ToString()?.ToUpper() ?? "", planilha.Cells[rows, 3].Value.ToString()?.ToUpper() ?? "").DataEstadoOperacional ?? "";
                                    var dataEstadoControle = _TesteOpticoRepository.Materiais(planilha.Cells[rows, 8].Value.ToString()?.ToUpper() ?? "", planilha.Cells[rows, 4].Value.ToString()?.ToUpper() ?? "", planilha.Cells[rows, 3].Value.ToString()?.ToUpper() ?? "").DataEstadoControle ?? "";

                                    testeOptico.CDO = planilha.Cells[rows, 8].Value.ToString()?.ToUpper() ?? "";
                                    testeOptico.NetwinId = GetCodViabilidadeEnderecosTotais(planilha.Cells[rows, 8].Value.ToString()?.ToUpper() ?? "", planilha.Cells[rows, 4].Value.ToString()?.ToUpper() ?? "", planilha.Cells[rows, 3].Value.ToString()?.ToUpper() ?? "");
                                    testeOptico.Fabricante = _TesteOpticoRepository.Materiais(planilha.Cells[rows, 8].Value.ToString()?.ToUpper() ?? "", planilha.Cells[rows, 4].Value.ToString()?.ToUpper() ?? "", planilha.Cells[rows, 3].Value.ToString()?.ToUpper() ?? "").Fabricante;
                                    testeOptico.Modelo = _TesteOpticoRepository.Materiais(planilha.Cells[rows, 8].Value.ToString()?.ToUpper() ?? "", planilha.Cells[rows, 4].Value.ToString()?.ToUpper() ?? "", planilha.Cells[rows, 3].Value.ToString()?.ToUpper() ?? "").Modelo;
                                    testeOptico.EstadoOperacional = _TesteOpticoRepository.Materiais(planilha.Cells[rows, 8].Value.ToString()?.ToUpper() ?? "", planilha.Cells[rows, 4].Value.ToString()?.ToUpper() ?? "", planilha.Cells[rows, 3].Value.ToString()?.ToUpper() ?? "").EstadoOperacional;
                                    if(dataEstadoOperacional != "")
                                    {
                                        testeOptico.DataEstadoOperacional = DateTime.Parse(dataEstadoOperacional);
                                    }
                                    testeOptico.EstadoControle = _TesteOpticoRepository.Materiais(planilha.Cells[rows, 8].Value.ToString()?.ToUpper() ?? "", planilha.Cells[rows, 4].Value.ToString()?.ToUpper() ?? "", planilha.Cells[rows, 3].Value.ToString()?.ToUpper() ?? "").EstadoControle;
                                    if (dataEstadoControle != "")
                                    {
                                        testeOptico.DataEstadoControle = DateTime.Parse(dataEstadoControle);
                                    }
                                    testeOptico.Endereco = _TesteOpticoRepository.Materiais(planilha.Cells[rows, 8].Value.ToString()?.ToUpper() ?? "", planilha.Cells[rows, 4].Value.ToString()?.ToUpper() ?? "", planilha.Cells[rows, 3].Value.ToString()?.ToUpper() ?? "").Endereco;
                                    //CONTADOR DE PROGRESSO IMPORTAÇÃO
                                    _progressBar.Progresso = rows * 100 / totalRows;
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -CDO- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 9].Value != null)
                                {
                                    testeOptico.Capacidade = int.Parse(planilha.Cells[rows, 9].Value.ToString()?.ToUpper() ?? "");
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -Capacidade- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 10].Value != null)
                                {
                                    testeOptico.TotalUms = int.Parse(planilha.Cells[rows, 10].Value.ToString()?.ToUpper() ?? "");
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -Total UMs- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 11].Value != null)
                                {
                                    testeOptico.EstadoCamposId = GetForeignKeyEstadoCampo(planilha.Cells[rows, 11].Value.ToString()?.ToUpper() ?? "");
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -Estado de Campo- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 12].Value != null)
                                {
                                    testeOptico.DatadeConstrucao = DateTime.FromOADate(double.Parse(planilha.Cells[rows, 12].Value.ToString()?.ToUpper() ?? ""));
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -Data de Construção- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 14].Value != null)
                                {
                                    testeOptico.EquipedeConstrucao = planilha.Cells[rows, 14].Value.ToString()?.ToUpper() ?? "";
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -Equipe de Contrução- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 15].Value != null)
                                {
                                    testeOptico.DatadoTeste = DateTime.FromOADate(double.Parse(planilha.Cells[rows, 15].Value.ToString()?.ToUpper() ?? ""));
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -Data do Teste- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 16].Value != null)
                                {
                                    testeOptico.DatadeRecebimento = DateTime.FromOADate(double.Parse(planilha.Cells[rows, 16].Value.ToString()?.ToUpper() ?? ""));
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -DATA DE ENVIO DO TESTE- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 18].Value != null)
                                {
                                    testeOptico.Tecnico = planilha.Cells[rows, 18].Value.ToString()?.ToUpper();
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -TÉCNICO DO TESTE- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 19].Value != null)
                                {
                                    testeOptico.PosicaoICX_DGO = planilha.Cells[rows, 19].Value.ToString()?.ToUpper();
                                }
                                else
                                {
                                    //ALERTA DE FALHA
                                    throw new Exception("A coluna -POSIÇÃO ICX/DGO- contém celula vazia. O preenchimento dessa coluna é obrigatória ");
                                }
                                if (planilha.Cells[rows, 20].Value != null)
                                {
                                    testeOptico.FibraDGO = planilha.Cells[rows, 20].Value.ToString()?.ToUpper();
                                }
                                if (planilha.Cells[rows, 21].Value != null)
                                {
                                    testeOptico.SplitterCEOS = planilha.Cells[rows, 21].Value.ToString()?.ToUpper();
                                }
                                if (planilha.Cells[rows, 22].Value != null)
                                {
                                    testeOptico.BobinadeLancamento = int.Parse(planilha.Cells[rows, 22].Value.ToString()?.ToUpper() ?? "");
                                }
                                if (planilha.Cells[rows, 23].Value != null)
                                {
                                    testeOptico.BobinadeRecepcao = int.Parse(planilha.Cells[rows, 23].Value.ToString()?.ToUpper() ?? "");
                                }
                                if (planilha.Cells[rows, 24].Value != null)
                                {
                                    testeOptico.QuantidadeDeTeste = int.Parse(planilha.Cells[rows, 24].Value.ToString()?.ToUpper() ?? "");
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
            ViewData["selectRegiao"] = _TesteOpticoRepository.Regiao();
            ViewData["selectEstacao"] = _TesteOpticoRepository.Estacoes();
            ViewData["selectEstado"] = _TesteOpticoRepository.Estado();
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
        public IActionResult Listar(int? pagina, string? regiao, string estado, string? estacao, string cdo, int? cabo, int? celula)
        {
            try
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
                IEnumerable<TesteOptico> listar = _TesteOpticoRepository.Listar(pagina, regiao, estado, estacao, cdo, cabo, celula);

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

                TempData["HstRegiao"] = testeOptico.Estacao.Estado.Regiao.Nome.ToString();
                TempData["HstEstacao"] = testeOptico.Estacao.NomeEstacao.ToString();
                TempData["HstEstado"] = testeOptico.Estacao.Estado.Nome.ToString();

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
        /*[HttpPost]
        public IActionResult Inserir(TesteOptico TesteOptico)
        {
          
            try
            {
                ViewData["selectViabilidade"] = _TesteOpticoRepository.Netwins();
                ViewData["selectConstrutora"] = _TesteOpticoRepository.Construtoras();
                ViewData["selectEstacao"] = _TesteOpticoRepository.Estacoes();
                ViewData["selectObras"] = _TesteOpticoRepository.TipoObras();
                ViewData["selectEstadoCampo"] = _TesteOpticoRepository.EstadoCampos();

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
        }*/
        [HttpPost]
        public IActionResult Editar(TesteOptico TesteOptico)
        {
            try
            {
                ViewData["selectViabilidade"] = _TesteOpticoRepository.Netwins();
                ViewData["selectConstrutora"] = _TesteOpticoRepository.Construtoras();
                ViewData["selectRegiao"] = _TesteOpticoRepository.Regiao();
                ViewData["selectEstado"] = _TesteOpticoRepository.Estado();
                ViewData["selectEstacao"] = _TesteOpticoRepository.Estacoes();
                ViewData["selectObras"] = _TesteOpticoRepository.TipoObras();
                ViewData["selectEstadoCampo"] = _TesteOpticoRepository.EstadoCampos();

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
