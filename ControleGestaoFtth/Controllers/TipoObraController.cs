using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ControleGestaoFtth.Controllers
{
    public class TipoObraController : Controller
    {
        private readonly ITipoObraRepository _tipoObraRepository;

        public TipoObraController(ITipoObraRepository tipoObraRepository)
        {
            _tipoObraRepository = tipoObraRepository;
        }
        public IActionResult Index()
        {
            ViewData["selectObras"] = _tipoObraRepository.Obras();
            return View();
        }
        public IActionResult Inserir()
        {
            return View();
        }
        public IActionResult Editar(int id)
        {
            TipoObra tipoObra = _tipoObraRepository.CarregarId(id);

            return View(tipoObra);
        }
        public IActionResult Confirmacao(int id)
        {
            TipoObra tipoObra = _tipoObraRepository.CarregarId(id);

            return View(tipoObra);
        }

        [HttpPost]
        public IActionResult Inserir(TipoObra tipoObra)
        {
            try
            {
                if (_tipoObraRepository.Listar().Any(p => p.Nome.Equals(tipoObra.Nome.ToUpper())))
                {
                    TempData["Falha"] = $"Tipo obra {tipoObra.Nome} já existe.";

                    return View(tipoObra);
                }
                else
                {
                    _tipoObraRepository.Cadastrar(tipoObra);
                    TempData["Sucesso"] = "Inserido com sucesso.";
                    return RedirectToAction("Inserir");
                }
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao inserir - {error}.";
                return View(tipoObra);
            }
        }
        [HttpPost]
        public IActionResult Editar(TipoObra tipoObra)
        {
            try
            {
                 _tipoObraRepository.Atualizar(tipoObra);
                 TempData["Sucesso"] = "Editado com sucesso.";
                 return RedirectToAction("Editar", new { id = tipoObra.Id });
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao editar - {error.Message}.";

                return View(tipoObra);
            }
        }

        [HttpGet]
        public IActionResult Apagar(int id)
        {
            try
            {
                if (!_tipoObraRepository.UniqueFk().Any(p => p.TipoObraId.Equals(id)))
                {
                    _tipoObraRepository.Deletar(id);
                    TempData["Sucesso"] = $"Excluído com sucesso.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Falha"] = $"Tipo Obra N°{id} contém registros relacionais e não pode ser excluída.";
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
                TipoObra tipoObra = _tipoObraRepository.CarregarId(id);

                return View(tipoObra);
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
                IEnumerable<TipoObra> listar = _tipoObraRepository.Listar(pagina, nome);

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
