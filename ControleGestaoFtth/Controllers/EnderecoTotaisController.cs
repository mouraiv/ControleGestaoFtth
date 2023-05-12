using ControleGestaoFtth.Repository;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ControleGestaoFtth.Controllers
{
    public class EnderecoTotaisController : Controller
    {
        private readonly IEnderecoTotaisRepository _enderecoTotaisRepository;

        public EnderecoTotaisController(IEnderecoTotaisRepository enderecoTotaisRepository)
        {
            _enderecoTotaisRepository = enderecoTotaisRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
