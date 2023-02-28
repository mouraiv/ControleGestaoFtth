using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;

namespace ControleGestaoFtth.Controllers
{
    public class TesteOpticoController : Controller
    {
        private readonly ITesteOpticoRepository _TesteOpticoRepository;
        private IWebHostEnvironment _env;

        public TesteOpticoController(ITesteOpticoRepository TesteOpticoRepository, IWebHostEnvironment env)
        {
            _TesteOpticoRepository = TesteOpticoRepository;
            _env = env;
        }
        public IActionResult Index()
        {
            ViewData["selectEstacao"] = _TesteOpticoRepository.Estacoes();

            return View();
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
