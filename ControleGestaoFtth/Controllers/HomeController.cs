﻿using Microsoft.AspNetCore.Mvc;

namespace ControleGestaoFtth.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

    }
}