using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ControleGestaoFtth.Controllers
{
    public class EstacaoController : Controller
    {
        private readonly IEstacoeRepository _estacaoRepository;

        public EstacaoController(IEstacoeRepository estacaoRepository)
        {
            _estacaoRepository = estacaoRepository;
        }
        public IActionResult Index()
        {
            ViewData["selectEstacao"] = _estacaoRepository.Estacao();
            ViewData["selectEstado"] = _estacaoRepository.Estado();
            return View();
        }
        public IActionResult Inserir()
        {
            ViewData["selectEstado"] = _estacaoRepository.Estado();
            return View();
        }
        public IActionResult Editar(int id)
        {
            ViewData["selectEstado"] = _estacaoRepository.Estado();
            Estacoe estacao = _estacaoRepository.CarregarId(id);

            return View(estacao);
        }
        public IActionResult Confirmacao(int id)
        {
            Estacoe estacao = _estacaoRepository.CarregarId(id);

            return View(estacao);
        }
        public IActionResult GetListDropDown(string estado)
        {
             if (estado != null)
            {
                var estacaoList = _estacaoRepository.Estacao(estado).Select(value => new Estacoe { NomeEstacao = value.NomeEstacao });
                return Json(new { estacao = estacaoList });
            }
            else
            {
                return Json(new
                {
                    estacao = _estacaoRepository.Estacao().Select(value => new Estacoe { NomeEstacao = value.NomeEstacao })
                });
            }
        }
        public IActionResult GetListMunicipios(string estado)
        {
            var municipiosList = _estacaoRepository.Municipio(estado);
            return Json(new { municipios = municipiosList }); 
        }

        [HttpPost]
        public IActionResult Inserir(Estacoe estacao)
        {
            try
            {
                ViewData["selectEstado"] = _estacaoRepository.Estado();

                if (_estacaoRepository.EstacaoExiste(estacao.NomeEstacao, estacao.Sigla))
                {
                    TempData["Falha"] = $"Estação {estacao.NomeEstacao} ou {estacao.Sigla} já existe, verificar.";

                    return View(estacao);

                }
                else
                {
                    _estacaoRepository.Cadastrar(estacao);
                    TempData["Sucesso"] = "Inserido com sucesso.";
                    return RedirectToAction("Inserir");
                }
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao inserir - {error}.";
                return View(estacao);
            }
        }
        [HttpPost]
        public IActionResult Editar(Estacoe estacao)
        {
            try
            {
                ViewData["selectEstado"] = _estacaoRepository.Estado();
                _estacaoRepository.Atualizar(estacao);
                TempData["Sucesso"] = "Editado com sucesso.";
                return RedirectToAction("Editar", new { id = estacao.Id });
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao editar - {error.Message}.";

                return View(estacao);
            }
        }

        [HttpGet]
        public IActionResult Apagar(int id)
        {
            try
            {
                if (!_estacaoRepository.UniqueFk().Any(p => p.EstacoesId.Equals(id)))
                {
                    _estacaoRepository.Deletar(id);
                    TempData["Sucesso"] = $"Estação excluída com sucesso.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Falha"] = $"Estação N°{id} contém registros relacionais e não pode ser excluída.";
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
                Estacoe estacao = _estacaoRepository.CarregarId(id);

                return View(estacao);
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro - {error.Message}.";
                return RedirectToAction("Index");
            }

        }

        [HttpGet]
        public IActionResult Listar(int? pagina, string estado, string estacao)
        {
            try
            {
                IEnumerable<Estacoe> listar = _estacaoRepository.Listar(pagina, estado, estacao);

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
