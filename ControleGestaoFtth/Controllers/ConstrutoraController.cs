using ControleGestaoFtth.Models;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

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
            return View();
        }
        public IActionResult Lista(int? pagina, string estacao)
        {
            ViewData["EstacaoId"] =
               new SelectList(_construtoraRepository.ListarEstacao(), "Id", "NomeEstacao");

            IEnumerable<Construtora> listar = _construtoraRepository.Listar(pagina, estacao);

            return PartialView(listar);
        }



    }
}
