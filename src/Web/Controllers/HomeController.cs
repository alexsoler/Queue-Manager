using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Interfaces;
using Web.Models;
using Web.ViewModels;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IIndexViewModel _indexViewModel;

        public HomeController(IIndexViewModel indexViewModel)
        {
            _indexViewModel = indexViewModel;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            await _indexViewModel.Loaded();

            return View(_indexViewModel.GetViewModel());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
