using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Interfaces;
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
        private readonly IAppLogger<HomeController> _logger;
        private readonly IWritableOptions<SystemCustom> _options;

        public HomeController(IIndexViewModel indexViewModel,
            IAppLogger<HomeController> logger,
            IWritableOptions<SystemCustom> options)
        {
            _indexViewModel = indexViewModel;
            _logger = logger;
            _options = options;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(RolesStatic.Admin) || User.IsInRole(RolesStatic.Operator))
            {
                await _indexViewModel.Loaded();

                _logger.LogInformation("Usuario Admin o Operator");
                return View(_indexViewModel.GetViewModel());
            }
            else if(User.IsInRole(RolesStatic.TouchScreen))
            {
                _logger.LogInformation("Usuario touch");
                return RedirectToAction("Index", "Touch");
            }
            else
            {
                _logger.LogInformation("Usuario display");
                return RedirectToAction("Index", "Display");
            }
        }

        public IActionResult About()
        {
            return View(_options.Value);
        }
        
        public IActionResult Contact()
        {
            return View();
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
