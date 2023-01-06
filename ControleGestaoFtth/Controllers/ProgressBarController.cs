using Microsoft.AspNetCore.Mvc;

namespace ControleGestaoFtth.Controllers
{
    public class ProgressBarController : Controller
    {
        public IActionResult Index()
        {
            double porcentage = 50.5;

            return View(porcentage);
        }
    }
}
