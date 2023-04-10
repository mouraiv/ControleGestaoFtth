using ControleGestaoFtth.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace ControleGestaoFtth.Controllers
{
    public class ProgressBarController : Controller
    {
        private readonly ProgressBar _progressBar;

        public ProgressBarController(ProgressBar progressBar)
        {
            _progressBar = progressBar;
        }
        public IActionResult Index()
        {
            return PartialView(_progressBar);
        }
    }
}
