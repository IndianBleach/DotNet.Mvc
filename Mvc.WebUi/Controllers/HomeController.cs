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
        private IUserRepository _userRepository;
        private IPageService _pageSerivce;


        public HomeController(
            ILogger<HomeController> logger,
            ApplicationContext ctx,
            ITagService tagService,
            CoreInterfaces.IAuthorizationService _authService,
            IIdeaRepository ideaRepo,
            IPageService pageService,
            IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _tagService = tagService;
            _authorizationService = _authService;
            _ideaRepository = ideaRepo;
            _pageSerivce = pageService;

        }

        public IActionResult About()
        {
            return View();
        }


        public async Task<IActionResult> Index(string? query, int? page)
        {
            HomeIdeasViewModel indexVm = new();

            if (page == null) page = 1;

            string currentUsername = User.Identity.Name != null ? User.Identity.Name : "";

            if (string.IsNullOrEmpty(query))
            {
                indexVm.Ideas = _ideaRepository.GetIdeas((int)page);
                indexVm.Pages = _pageSerivce.GeneratePages((int)page, _ideaRepository.GetCount(), 10);
            }
            else
            {
                indexVm.Ideas = _ideaRepository.GetIdeas(query, (int)page).ToList();
                indexVm.Pages = _pageSerivce.GeneratePages((int)page, _ideaRepository.GetCount(query), 10);
            }
            
            indexVm.Recommends = _ideaRepository.GetRecommendIdeas(currentUsername).ToList();
            indexVm.IdeasNeedMembers = _ideaRepository.GetSideIdeasByStatusFilter(IdeaStatuses.FindMembers).ToList();
            indexVm.SearchTags = _tagService.GetAllTags().Take(5).ToList();
            indexVm.News = await _userRepository.GetNewsAsync();

            return View(indexVm);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}