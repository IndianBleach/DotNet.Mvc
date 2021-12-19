using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.Entities.IdeaEntity;
using Mvc.ApplicationCore.Interfaces;
using Mvc.Infrastructure.Data;
using Mvc.WebUi.Models;
using Mvc.WebUi.ViewModels;
using System.Diagnostics;
using CoreInterfaces = Mvc.ApplicationCore.Interfaces;

namespace Mvc.WebUi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly ApplicationContext _ctx;
        private readonly ITagService _tagService;
        private readonly CoreInterfaces.IAuthorizationService _authorizationService;
        private IIdeaRepository _ideaRepository;
        private IPageService _pageSerivce;


        public HomeController(
            ILogger<HomeController> logger,
            ApplicationContext ctx,
            ITagService tagService,
            CoreInterfaces.IAuthorizationService _authService,
            IIdeaRepository ideaRepo,
            IPageService pageService)
        {
            _logger = logger;
            //_ctx = ctx;
            _tagService = tagService;
            _authorizationService = _authService;
            _ideaRepository = ideaRepo;
            _pageSerivce = pageService;

        }


        //[Authorize]
        public async Task<IActionResult> Index(string? query, int? page)
        {
            HomeIdeasViewModel indexVm = new();

            if (page == null) page = 1;


            if (string.IsNullOrEmpty(query))
            {
                indexVm.Ideas = _ideaRepository.GetIdeasPerPage((int)page).ToList();
            }
            else
            {
                indexVm.Ideas = _ideaRepository.GetIdeasWithQuery(query, (int)page).ToList();
            }

            indexVm.Pages = _pageSerivce.GeneratePages((int)page, _ideaRepository.GetCount(), 10);
            indexVm.Recommends = _ideaRepository.GetRecommendIdeas(User.Identity.Name).ToList();
            indexVm.IdeasNeedMembers = _ideaRepository.GetSideIdeasByStatusFilter(IdeaStatuses.FindMembers).ToList();
            indexVm.SearchTags = _tagService.GetAllTags().Take(5).ToList();

            return View(indexVm);
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