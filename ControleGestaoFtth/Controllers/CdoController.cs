using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ControleGestaoFtth.Controllers
{
    public class CdoController : Controller
    {
        private readonly ICdoRepository _cdoRepository;

        public CdoController(ICdoRepository cdoRepository)
        {
            _cdoRepository = cdoRepository;
        }
        public IActionResult Index()
        {
            ViewData["selectEstacao"] = _cdoRepository.Estacoes();

            return View();
        }
        public IActionResult Inserir()
        {
            ViewData["selectEstacao"] = _cdoRepository.Estacoes();
            ViewData["selectObras"] = _cdoRepository.TipoObras();

            return View();
        }
        public IActionResult Editar(int id)
        {
            Cdo cdo = _cdoRepository.CarregarId(id);
            ViewData["selectEstacao"] = _cdoRepository.Estacoes();
            ViewData["selectObras"] = _cdoRepository.TipoObras();

            return View(cdo);
        }
        public IActionResult Confirmacao(int id)
        {
            Cdo cdo = _cdoRepository.CarregarId(id);

            return View(cdo);
        }
        [HttpGet]
        public IActionResult Listar(int? pagina, string estacao, string cdo, int? cabo, int? celula)
        {
            if (estacao != null)
            {
                ViewData["selectCdoFilter"] = _cdoRepository.FilterCdo(estacao);
                ViewData["selectCaboFilter"] = _cdoRepository.FilterCabo(estacao);
                ViewData["selectCelulaFilter"] = _cdoRepository.FilterCelula(estacao);
            }
            else
            {
                ViewData["selectCdoFilter"] = _cdoRepository.FilterCdo("");
                ViewData["selectCaboFilter"] = _cdoRepository.FilterCabo("");
                ViewData["selectCelulaFilter"] = _cdoRepository.FilterCelula("");
            }

            try
            {
                IEnumerable<Cdo> listar = _cdoRepository.Listar(pagina, estacao ?? "", cdo, cabo, celula);

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
                Cdo cdo = _cdoRepository.CarregarId(id);

                return View(cdo);
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao listar - {error.Message}.";
                return RedirectToAction("Index");
            }

        }
        [HttpPost]
        public IActionResult Inserir(Cdo cdos)
        {
            ViewData["selectEstacao"] = _cdoRepository.Estacoes();
            ViewData["selectObras"] = _cdoRepository.TipoObras();

            try
            {
                if (_cdoRepository.UniqueCdo().Any(p => p.CDO.Equals(cdos.CDO.ToUpper())))
                {
                        TempData["Falha"] = $"CDO {cdos.CDO} já existe.";

                        return View(cdos);
                 }
                 else
                 {
                        _cdoRepository.Cadastrar(cdos);
                        TempData["Sucesso"] = "Inserido com sucesso.";
                        return RedirectToAction("Inserir");
                 }
                
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao inserir - {error.Message}.";
                return View(cdos);
            }
        }
        [HttpPost]
        public IActionResult Editar(Cdo cdos)
        {
            try
            {
                _cdoRepository.Atualizar(cdos);
                TempData["Sucesso"] = "Editado com sucesso.";
                return RedirectToAction("Editar", new { id = cdos.Id });

            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao editar - {error.Message}.";
                return View(cdos);
            }
        }
        [HttpGet]
        public IActionResult Apagar(int id)
        {
            try
            {
                _cdoRepository.Deletar(id);
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
