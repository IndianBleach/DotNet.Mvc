using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.Interfaces;
using Mvc.Infrastructure.Data;
using Mvc.WebUi.Models;
using System.Diagnostics;
using CoreInterfaces = Mvc.ApplicationCore.Interfaces;

namespace Mvc.WebUi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _ctx;
        private readonly ITagService _tagService;
        private readonly CoreInterfaces.IAuthorizationService _authorizationService;
        private IIdeaRepository _ideaRepository;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationContext ctx,
            ITagService tagService,
            CoreInterfaces.IAuthorizationService _authService,
            IIdeaRepository ideaRepo)
        {
            _logger = logger;
            _ctx = ctx;
            _tagService = tagService;
            _authorizationService = _authService;
            _ideaRepository = ideaRepo;

            //ViewBag.Tags = _tagService.GetAllTags();
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var res = _ideaRepository.GetIdeasPerPage(1);

            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return Content("Secret WeB pAGE");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}