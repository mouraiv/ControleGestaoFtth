using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ControleGestaoFtth.Controllers
{
    public class TesteOpticoController : Controller
    {
        private readonly ITesteOpticoRepository _testeOpticoRepository;

        public TesteOpticoController(ITesteOpticoRepository testeOpticoRepository)
        {
            _testeOpticoRepository = testeOpticoRepository;
        }
        public IActionResult Index()
        {
            ViewData["selectEstacao"] = _testeOpticoRepository.Estacoes();

            return View();
        }
        public IActionResult Inserir()
        {
            ViewData["selectViabilidade"] = _testeOpticoRepository.Netwins();
            ViewData["selectEstacao"] = _testeOpticoRepository.Estacoes();
            ViewData["selectObras"] = _testeOpticoRepository.TipoObras();
            ViewData["selectEstadoCampo"] = _testeOpticoRepository.EstadoCampos();

            return View();
        }
        public IActionResult Editar(int id)
        {
            TesteOptico testeOptico = _testeOpticoRepository.CarregarId(id);
            ViewData["selectViabilidade"] = _testeOpticoRepository.Netwins();
            ViewData["selectEstacao"] = _testeOpticoRepository.Estacoes();
            ViewData["selectObras"] = _testeOpticoRepository.TipoObras();
            ViewData["selectEstadoCampo"] = _testeOpticoRepository.EstadoCampos();

            return View(testeOptico);
        }
        public IActionResult Confirmacao(int id)
        {
            TesteOptico testeOptico = _testeOpticoRepository.CarregarId(id);

            return View(testeOptico);
        }
        [HttpGet]
        public IActionResult Listar(int? pagina, string estacao, string cdo, int? cabo, int? celula)
        {
            if (estacao != null)
            {
                ViewData["selectCdoFilter"] = _testeOpticoRepository.FilterCdo(estacao);
                ViewData["selectCaboFilter"] = _testeOpticoRepository.FilterCabo(estacao);
                ViewData["selectCelulaFilter"] = _testeOpticoRepository.FilterCelula(estacao);
            }
            else
            {
                ViewData["selectCdoFilter"] = _testeOpticoRepository.FilterCdo("");
                ViewData["selectCaboFilter"] = _testeOpticoRepository.FilterCabo("");
                ViewData["selectCelulaFilter"] = _testeOpticoRepository.FilterCelula("");
            }

            try
            {
                IEnumerable<TesteOptico> listar = _testeOpticoRepository.Listar(pagina, estacao ?? "", cdo, cabo, celula);

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
                TesteOptico testeOptico = _testeOpticoRepository.CarregarId(id);

                return View(testeOptico);
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao listar - {error.Message}.";
                return RedirectToAction("Index");
            }

        }
        [HttpPost]
        public IActionResult Inserir(TesteOptico testeOptico)
        {
            ViewData["selectViabilidade"] = _testeOpticoRepository.Netwins();
            ViewData["selectEstacao"] = _testeOpticoRepository.Estacoes();
            ViewData["selectObras"] = _testeOpticoRepository.TipoObras();
            ViewData["selectEstadoCampo"] = _testeOpticoRepository.EstadoCampos();

            try
            {
               
               _testeOpticoRepository.Cadastrar(testeOptico);
               TempData["Sucesso"] = "Inserido com sucesso.";
               return RedirectToAction("Inserir");
                   
               
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao inserir - {error.Message}.";
                return View(testeOptico);
            }
        }
        [HttpPost]
        public IActionResult Editar(TesteOptico testeOptico)
        {
            try
            {
                _testeOpticoRepository.Atualizar(testeOptico);
                TempData["Sucesso"] = "Editado com sucesso.";
                return RedirectToAction("Editar", new { id = testeOptico.Id });

            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao editar - {error.Message}.";
                return View(testeOptico);
            }
        }
        [HttpGet]
        public IActionResult Apagar(int id)
        {
            try
            {
                _testeOpticoRepository.Deletar(id);
                TempData["Sucesso"] = "CDO Excluída com sucesso.";
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