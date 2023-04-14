using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ControleGestaoFtth.Controllers
{
    public class EstadoCampoController : Controller
    {
        private readonly IEstadoCampoRepository _estadoCampoRepository;

        public EstadoCampoController(IEstadoCampoRepository estadoCampoRepository)
        {
            _estadoCampoRepository = estadoCampoRepository;
        }
        public IActionResult Index()
        {
            ViewData["selectCampo"] = _estadoCampoRepository.Campo();
            return View();
        }
        public IActionResult Inserir()
        {
            return View();
        }
        public IActionResult Editar(int id)
        {
            EstadoCampo estadoCampo = _estadoCampoRepository.CarregarId(id);

            return View(estadoCampo);
        }
        public IActionResult Confirmacao(int id)
        {
            EstadoCampo estadoCampo = _estadoCampoRepository.CarregarId(id);

            return View(estadoCampo);
        }

        [HttpPost]
        public IActionResult Inserir(EstadoCampo estadoCampo)
        {
            try
            {
                if (_estadoCampoRepository.EstadoCampoExiste(estadoCampo.Nome))
                {
                    TempData["Falha"] = $"Estado Campo {estadoCampo.Nome} já existe.";

                    return View(estadoCampo);
                }
                else
                {
                    _estadoCampoRepository.Cadastrar(estadoCampo);
                    TempData["Sucesso"] = "Inserido com sucesso.";
                    return RedirectToAction("Inserir");
                }
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao inserir - {error}.";
                return View(estadoCampo);
            }
        }
        [HttpPost]
        public IActionResult Editar(EstadoCampo estadoCampo)
        {
            try
            {
                _estadoCampoRepository.Atualizar(estadoCampo);
                TempData["Sucesso"] = "Editado com sucesso.";
                return RedirectToAction("Editar", new { id = estadoCampo.Id });
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao editar - {error.Message}.";

                return View(estadoCampo);
            }
        }

        [HttpGet]
        public IActionResult Apagar(int id)
        {
            try
            {
                if (!_estadoCampoRepository.UniqueFk().Any(p => p.EstadoCamposId.Equals(id)))
                {
                    _estadoCampoRepository.Deletar(id);
                    TempData["Sucesso"] = $"Excluído com sucesso.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Falha"] = $"Estado Campo N°{id} contém registros relacionais e não pode ser excluída.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao excluir - {error.Message}.";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Detalhe(int id)
        {
            try
            {
                EstadoCampo estadoCampo = _estadoCampoRepository.CarregarId(id);

                return View(estadoCampo);
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro - {error.Message}.";
                return RedirectToAction("Index");
            }

        }

        [HttpGet]
        public IActionResult Listar(int? pagina, string nome)
        {
            try
            {
                IEnumerable<EstadoCampo> listar = _estadoCampoRepository.Listar(pagina, nome);

                return PartialView(listar);
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro - {error.Message}.";
                return PartialView();
            }


        }
    }
}
