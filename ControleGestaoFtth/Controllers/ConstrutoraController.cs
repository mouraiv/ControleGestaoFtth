using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository;
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
            ViewData["selectConstrutora"] = _construtoraRepository.Obras();
            return View();
        }
        public IActionResult Inserir()
        {
            return View();
        }
        public IActionResult Editar(int id)
        {
            Construtora Construtora = _construtoraRepository.CarregarId(id);

            return View(Construtora);
        }
        public IActionResult Confirmacao(int id)
        {
            Construtora Construtora = _construtoraRepository.CarregarId(id);

            return View(Construtora);
        }

        [HttpPost]
        public IActionResult Inserir(Construtora Construtora)
        {
            try
            {
                if (_construtoraRepository.ContrutoraExiste(Construtora.Nome))
                {
                    TempData["Falha"] = $"Tipo obra {Construtora.Nome} já existe.";

                    return View(Construtora);
                }
                else
                {
                    _construtoraRepository.Cadastrar(Construtora);
                    TempData["Sucesso"] = "Inserido com sucesso.";
                    return RedirectToAction("Inserir");
                }
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao inserir - {error}.";
                return View(Construtora);
            }
        }
        [HttpPost]
        public IActionResult Editar(Construtora Construtora)
        {
            try
            {
                _construtoraRepository.Atualizar(Construtora);
                TempData["Sucesso"] = "Editado com sucesso.";
                return RedirectToAction("Editar", new { id = Construtora.Id });
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao editar - {error.Message}.";

                return View(Construtora);
            }
        }

        [HttpGet]
        public IActionResult Apagar(int id)
        {
            try
            {
                if (!_construtoraRepository.UniqueFk().Any(p => p.ConstrutorasId.Equals(id)))
                {
                    _construtoraRepository.Deletar(id);
                    TempData["Sucesso"] = $"Excluído com sucesso.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Falha"] = $"Construtora N°{id} contém registros relacionais e não pode ser excluída.";
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
                Construtora Construtora = _construtoraRepository.CarregarId(id);

                return View(Construtora);
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
                IEnumerable<Construtora> listar = _construtoraRepository.Listar(pagina, nome);

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
