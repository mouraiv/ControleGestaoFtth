using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

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
            ViewData ["selectEstacao"] = _estacaoRepository.Listar();
            ViewData["selectResponsavel"] = _estacaoRepository.Responsavel();
            return View();
        }
        public IActionResult Inserir()
        {
            return View();
        }
        public IActionResult Editar(int id)
        {
            Estacoe estacao = _estacaoRepository.CarregarId(id);

            return View(estacao);
        }
        public IActionResult Confirmacao(int id)
        {
            Estacoe estacao = _estacaoRepository.CarregarId(id);

            return View(estacao);
        }

        [HttpPost]
        public IActionResult Inserir(Estacoe estacao)
        {
            try
            {
                if (_estacaoRepository.Listar().Any(p => p.NomeEstacao.Equals(estacao.NomeEstacao.ToUpper())))
                {
                    TempData["Falha"] = $"O estação {estacao.NomeEstacao} já existe!";

                    return View(estacao);
                }
                else
                {
                    _estacaoRepository.Cadastrar(estacao);
                    TempData["Sucesso"] = "Cadastrado com sucesso!.";
                    return RedirectToAction("Inserir");
                }
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao inserir - {error}";
                return View(estacao);
            }
        }
        [HttpPost]
        public IActionResult Editar(Estacoe estacao)
        {
            try
            {
                _estacaoRepository.Atualizar(estacao);
                TempData["Sucesso"] = "Atualizado com sucesso!";
                return RedirectToAction("Editar", new { id = estacao.Id });

            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao Atualizar - {error.Message}";
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
                    TempData["Sucesso"] = $"Estação Excluída com sucesso.";
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
                TempData["Falha"] = $"Erro ao Excluir - {error.Message}";
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
                TempData["Falha"] = $"Erro - {error.Message}";
                return PartialView();
            }

        }

        [HttpGet]
        public IActionResult Listar(int? pagina, string nomeEstacao, string responsavel)
        {
            try
            {
                IEnumerable<Estacoe> listar = _estacaoRepository.Listar(pagina, nomeEstacao, responsavel);

                return PartialView(listar);
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro - {error.Message}";
                return PartialView();
            }

                
        }
    }
}
