using ControleGestaoFtth.Models;
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
        public IActionResult Index(int? pagina)
        {
            IEnumerable<Construtora> construtoras = _construtoraRepository.Listar(pagina);

            return View(construtoras);
        }
    }
}
