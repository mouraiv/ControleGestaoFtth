using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Confirmacao(int id)
        {
            Construtora construtora = _construtoraRepository.CarregarId(id);

            return View(construtora);
        }
        [HttpGet]
        public IActionResult Listar(int? pagina, string estacao, string cdo, int? cabo, int? celula)
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

            try
            {
                IEnumerable<Construtora> listar = _construtoraRepository.Listar(pagina, estacao ?? "", cdo, cabo, celula);

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
                Construtora construtora = _construtoraRepository.CarregarId(id);

                return View(construtora);
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao listar - {error.Message}.";
                return RedirectToAction("Index");
            }

        }
        [HttpPost]
        public IActionResult Inserir(Construtora construtora)
        {
            ViewData["selectViabilidade"] = _construtoraRepository.Netwins();
            ViewData["selectEstacao"] = _construtoraRepository.Estacoes();
            ViewData["selectObras"] = _construtoraRepository.TipoObras();
            ViewData["selectEstadoCampo"] = _construtoraRepository.EstadoCampos();

            try
            {
                if (ModelState.IsValid)
                {
                    if (_construtoraRepository.UniqueCdo().Any(p => p.CDO.Equals(construtora.CDO.ToUpper())))
                    {
                        TempData["Falha"] = $"CDO {construtora.CDO} já existe.";

                        return View(construtora);
                    }
                    else
                    {
                        _construtoraRepository.Cadastrar(construtora);
                        TempData["Sucesso"] = "Inserido com sucesso.";
                        return RedirectToAction("Inserir");
                    }
                }
                return View(construtora);
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao inserir - {error.Message}.";
                return View(construtora);
            }
        }
        [HttpPost]
        public IActionResult Editar(Construtora construtora)
        {
            try
            {
                _construtoraRepository.Atualizar(construtora);
                TempData["Sucesso"] = "Editado com sucesso.";
                return RedirectToAction("Editar", new { id = construtora.Id });
                
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao editar - {error.Message}.";
                return View(construtora);
            }
        }
        [HttpGet]
        public IActionResult Apagar(int id)
        {
            try
            {
                _construtoraRepository.Deletar(id);
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
