using Microsoft.AspNetCore.Mvc;

namespace ControleGestaoFtth.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
