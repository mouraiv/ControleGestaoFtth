using ControleGestaoFtth.ComponentModel;
using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Diagnostics;

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
            //ArquivoModel arquivos = new ();

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
