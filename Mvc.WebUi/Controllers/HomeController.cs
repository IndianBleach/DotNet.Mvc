using Microsoft.AspNetCore.Mvc;
using Mvc.ApplicationCore.Interfaces;
using Mvc.Infrastructure.Data;
using Mvc.Infrastructure.Services;
using Mvc.WebUi.Models;
using System.Diagnostics;

namespace Mvc.WebUi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _ctx;
        private readonly ITagService _tagService;

        public HomeController(ILogger<HomeController> logger, ApplicationContext ctx, ITagService tagService)
        {
            _logger = logger;
            _ctx = ctx;
            _tagService = tagService;
        }

        public IActionResult Index()
        {
            return Json(_tagService.GetPopularTags());
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