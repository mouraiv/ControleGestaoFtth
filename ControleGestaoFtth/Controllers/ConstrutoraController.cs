using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ControleGestaoFtth.Controllers
{
    public class ConstrutoraController : Controller
    {
        private readonly IConstrutoraRepository _construtoraRepository;

        public ConstrutoraController(IConstrutoraRepository construtoraRepository)
        {
            _construtoraRepository = construtoraRepository;
        }
        public IActionResult Index()
        {
            ViewData["selectEstacao"] = _construtoraRepository.Estacoes();
            return View();
        }
        public IActionResult Inserir()
        {
            ViewData["selectViabilidade"] = _construtoraRepository.Netwins();
            ViewData["selectEstacao"] = _construtoraRepository.Estacoes();
            ViewData["selectObras"] = _construtoraRepository.TipoObras();
            ViewData["selectEstadoCampo"] = _construtoraRepository.EstadoCampos();

            return View();
        }
        public IActionResult Editar(int id)
        {
            Construtora construtora = _construtoraRepository.CarregarId(id);
            ViewData["selectViabilidade"] = _construtoraRepository.Netwins();
            ViewData["selectEstacao"] = _construtoraRepository.Estacoes();
            ViewData["selectObras"] = _construtoraRepository.TipoObras();
            ViewData["selectEstadoCampo"] = _construtoraRepository.EstadoCampos();

            return View(construtora);
        }
        [HttpGet]
        public IActionResult Lista(int? pagina, string estacao, string cdo, int? cabo, int? celula)
        {
            if (estacao != null)
            {
                ViewData["selectCdoFilter"] = _construtoraRepository.FilterCdo(estacao);
                ViewData["selectCaboFilter"] = _construtoraRepository.FilterCabo(estacao);
                ViewData["selectCelulaFilter"] = _construtoraRepository.FilterCelula(estacao);
            }
            else
            {
                ViewData["selectCdoFilter"] = _construtoraRepository.FilterCdo("");
                ViewData["selectCaboFilter"] = _construtoraRepository.FilterCabo("");
                ViewData["selectCelulaFilter"] = _construtoraRepository.FilterCelula("");
            }

            IEnumerable<Construtora> listar = _construtoraRepository.Listar(pagina, estacao ?? "", cdo, cabo, celula);

            return PartialView(listar);
        }
        [HttpGet]
        public IActionResult Detalhe(int id)
        {
            Construtora construtora = _construtoraRepository.CarregarId(id);

            return View(construtora);

        }
        [HttpPost]
        public IActionResult Editar(Construtora construtora)
        {
            try
            {
                _construtoraRepository.Atualizar(construtora);
                TempData["Sucesso"] = "Atualizado com sucesso!";
                return RedirectToAction("Editar", new { id = construtora.Id });
                
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao Atualizar - {error.Message}";
                return View(construtora);
            }
        }
    }
}
