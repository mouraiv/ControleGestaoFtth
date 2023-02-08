using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ControleGestaoFtth.Controllers
{
    public class NetwinController : Controller
    {
        private readonly INetwinRepository _netwinRepository;

        public NetwinController(INetwinRepository netwinRepository)
        {
            _netwinRepository = netwinRepository;
        }
        public IActionResult Index()
        {
            ViewData["selectNetwin"] = _netwinRepository.Descricao();
            return View();
        }
        public IActionResult Inserir()
        {
            return View();
        }
        public IActionResult Editar(int id)
        {
            Netwin netwin = _netwinRepository.CarregarId(id);

            return View(netwin);
        }
        public IActionResult Confirmacao(int id)
        {
            Netwin netwin = _netwinRepository.CarregarId(id);

            return View(netwin);
        }

        [HttpPost]
        public IActionResult Inserir(Netwin netwin)
        {
            try
            {
                if (_netwinRepository.Listar().Any(p => p.Codigo.Equals(netwin.Codigo)))
                {
                    TempData["Falha"] = $"Codigo {netwin.Codigo} já existe.";

                    return View(netwin);
                }
                else
                {
                    _netwinRepository.Cadastrar(netwin);
                    TempData["Sucesso"] = "Inserido com sucesso.";
                    return RedirectToAction("Inserir");
                }
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao inserir - {error}.";
                return View(netwin);
            }
        }
        [HttpPost]
        public IActionResult Editar(Netwin netwin)
        {
            try
            {
                 _netwinRepository.Atualizar(netwin);
                 TempData["Sucesso"] = "Editado com sucesso.";
                 return RedirectToAction("Editar", new { id = netwin.Id });
              
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao editar - {error.Message}.";

                return View(netwin);
            }
        }

        [HttpGet]
        public IActionResult Apagar(int id)
        {
            try
            {
                if (!_netwinRepository.UniqueFk().Any(p => p.NetwinId.Equals(id)))
                {
                    _netwinRepository.Deletar(id);
                    TempData["Sucesso"] = $"Excluído com sucesso.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Falha"] = $"Codigo referente a id N°{id} contém registros relacionais e não pode ser excluída.";
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
                Netwin netwin = _netwinRepository.CarregarId(id);
                return View(netwin);
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro - {error.Message}.";
                return RedirectToAction("Index");
            }

        }

        [HttpGet]
        public IActionResult Listar(int? pagina, string descricao)
        {
            try
            {
                IEnumerable<Netwin> listar = _netwinRepository.Listar(pagina, descricao);

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
