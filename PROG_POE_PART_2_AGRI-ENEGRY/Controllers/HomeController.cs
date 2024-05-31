using Microsoft.AspNetCore.Mvc;
using PROG_POE_PART_2_AGRI_ENEGRY.Models;
using System.Diagnostics;

namespace PROG_POE_PART_2_AGRI_ENEGRY.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        //Amethod to return the view for the about page
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Market()
        {
            return View();
        }
        public IActionResult EducationalResources()
        {
            return View();
        }
        public IActionResult Hub()
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
