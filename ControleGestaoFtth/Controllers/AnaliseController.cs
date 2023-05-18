using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ControleGestaoFtth.Controllers
{
    public class AnaliseController : Controller
    {
        private readonly IAnaliseRepository _analiseRepository;

        public AnaliseController(IAnaliseRepository analiseRepository)
        {
            _analiseRepository = analiseRepository;
        }

        public IActionResult Index()
        {
            ViewData["selectRegiao"] = _analiseRepository.Regiao();
            ViewData["selectEstado"] = _analiseRepository.Estado();
            ViewData["selectEstacao"] = _analiseRepository.Estacoes("");

            return View();
        }
        [HttpGet]
        public IActionResult Listar(int? pagina, string? regiao, string estado, string? estacao, int? cdo, int? tecnico, int? status)
        {
            try
            {
                var statusList = new[]
                        {
                            new SelectListItem { Value = "1", Text = "APROVADO" },
                            new SelectListItem { Value = "0", Text = "REPROVADO" }
                        };
                ViewData["selectStatusFilter"] = statusList;

                if (estacao != null)
                {
                    ViewData["selectCdoFilter"] = _analiseRepository.TesteOptico(estacao);
                    ViewData["selectTecnicoFilter"] = _analiseRepository.Tecnico();
                }
                else
                {
                    ViewData["selectCdoFilter"] = _analiseRepository.TesteOptico("");
                    ViewData["selectTecnicoFilter"] = _analiseRepository.Tecnico();
                }

                IEnumerable<Analise> listar = _analiseRepository.Listar(pagina, regiao, estado, estacao, cdo, tecnico, status);

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
                Analise analise = _analiseRepository.CarregarId(id);

                //TempData["HstRegiao"] = testeOptico.Estacao.Estado.Regiao.Nome.ToString();
                //TempData["HstEstacao"] = testeOptico.Estacao.NomeEstacao.ToString();
                //TempData["HstEstado"] = testeOptico.Estacao.Estado.Nome.ToString();

                return View(analise);
            }
            catch (Exception error)
            {
                TempData["Falha"] = $"Erro ao listar - {error.Message}.";

                return RedirectToAction("Index");
            }

        }
        public IActionResult GetListDropDown(string regiao, string estado, string estacao)
        {
            if (regiao != null && estado == null)
            {
                var estadoList = _analiseRepository.Estado(regiao).Select(value => new Estado { Nome = value.Nome });
                return Json(new { estado = estadoList });

            }
            else if (estado != null)
            {
                var estacaoList = _analiseRepository.Estacoes(estado).Select(value => new Estacoe { NomeEstacao = value.NomeEstacao });
                return Json(new { estacao = estacaoList });
            }
            else
            {
                return Json(new
                {
                    estado = _analiseRepository.Estado().Select(value => new Estado { Nome = value.Nome }),
                    estacao = _analiseRepository.Estacoes("").Select(value => new Estacoe { NomeEstacao = value.NomeEstacao })
                });
            }
        }
    }
}
