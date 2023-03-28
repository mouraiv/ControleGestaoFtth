﻿using ControleGestaoFtth.ComponentModel;
using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;

namespace ControleGestaoFtth.Controllers
{
    public class TesteOpticoController : Controller
    {
        private readonly ITesteOpticoRepository _TesteOpticoRepository;
        private readonly ConversionViewModel _conversionViewModel;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TesteOpticoController(ITesteOpticoRepository TesteOpticoRepository, ConversionViewModel conversionViewModel, IWebHostEnvironment webHostEnvironment)
        {
            _TesteOpticoRepository = TesteOpticoRepository;
            _conversionViewModel = conversionViewModel;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            ViewData["selectEstacao"] = _TesteOpticoRepository.Estacoes();

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
        private int GetForeignKeyConstrutora(string value)
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
        private int GetForeignKeyEstacao(string value)
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
        private int GetForeignKeyTipoObra(string value)
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
        private int GetForeignKeyEstadoCampo(string value)
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
        [HttpPost]
        public IActionResult Importar(IFormFile file)
        {
            // Ler os dados do arquivo XLSX
            var dados = new List<TesteOptico>();

            using (var pacote = new OfficeOpenXml.ExcelPackage(file.OpenReadStream()))
            {
                var planilha = pacote.Workbook.Worksheets[1];

                for (int i = planilha.Dimension.Start.Row + 1; i <= planilha.Dimension.End.Row; i++)
                {
                    var testeOptico = new TesteOptico();
                  
                    testeOptico.ConstrutorasId = GetForeignKeyConstrutora(planilha.Cells[i, 2].Value.ToString() ?? ""); ;
                    testeOptico.EstacoesId = GetForeignKeyEstacao(planilha.Cells[i, 2].Value.ToString() ?? ""); 
                    testeOptico.TipoObraId = GetForeignKeyTipoObra(planilha.Cells[i, 3].Value.ToString() ?? ""); 
                    testeOptico.Cabo = (int)planilha.Cells[i, 4].Value;
                    testeOptico.Celula = (int)planilha.Cells[i, 5].Value;
                    testeOptico.CDO = planilha.Cells[i, 6].Value.ToString() ?? "";
                    testeOptico.Capacidade = (int)planilha.Cells[i, 7].Value;
                    testeOptico.TotalUms = (int)planilha.Cells[i, 8].Value;
                    testeOptico.EstadoCamposId = GetForeignKeyEstadoCampo(planilha.Cells[i, 9].Value.ToString() ?? "");
                    testeOptico.DatadeConstrucao = DateTime.Parse(planilha.Cells[i, 10].Value.ToString() ?? "");
                    testeOptico.EquipedeConstrucao = planilha.Cells[i, 11].Value.ToString();
                    testeOptico.DatadoTeste = DateTime.Parse(planilha.Cells[i, 12].Value.ToString() ?? "");
                    //DatadeEnvio = 13 (implementar no modelo)
                    testeOptico.Tecnico = planilha.Cells[i, 14].Value.ToString();
                    testeOptico.PosicaoICX_DGO = planilha.Cells[i, 15].Value.ToString();
                    testeOptico.FibraDGO = planilha.Cells[i, 16].Value.ToString();
                    testeOptico.SplitterCEOS = planilha.Cells[i, 17].Value.ToString();
                    testeOptico.BobinadeLancamento = (int)planilha.Cells[i, 18].Value;
                    testeOptico.BobinadeRecepcao = (int)planilha.Cells[i, 19].Value;
                    testeOptico.QuantidadeDeTeste = (int)planilha.Cells[i, 20].Value;
                    dados.Add(testeOptico);
                }
            }

            // Salvar os dados no banco de dados
            foreach (var optico in dados)
            {
                _TesteOpticoRepository.Cadastrar(optico);
            }
            
            // Redirecionar o usuário de volta para a página inicial
            return RedirectToAction("Index");
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
