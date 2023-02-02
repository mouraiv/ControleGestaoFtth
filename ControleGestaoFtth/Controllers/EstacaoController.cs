using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository;
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

        [HttpPost]
        public IActionResult Inserir(Estacoe estacao)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Detalhe(int id)
        {
            Estacoe estacao = _estacaoRepository.CarregarId(id);

            return View(estacao);

        }

        [HttpGet]
        public IActionResult Listar(int? pagina, string nomeEstacao, string responsavel)
        {

            IEnumerable<Estacoe> listar = _estacaoRepository.Listar(pagina, nomeEstacao, responsavel);

            return PartialView(listar);
        }
    }
}
