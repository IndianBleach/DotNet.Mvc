using Microsoft.AspNetCore.Mvc;
using Mvc.ApplicationCore.Interfaces;
using Mvc.WebUi.ViewModels;

namespace Mvc.WebUi.Controllers
{
    public class FindController : Controller
    {
        private readonly ITagService _tagService;
        private readonly IUserRepository _userRepository;
        private readonly IPageService _pageService;

        public FindController(ITagService tagService, IUserRepository userRepo, IPageService pageService)
        {
            _tagService= tagService;
            ViewBag.Tags = _tagService.GetAllTags();
            _userRepository = userRepo;
            _pageService = pageService;
        }        


        //[HttpGet]
        public IActionResult Index(string? query, int? page)
        {
            if (page == null) page = 1;

            FindMembersViewModel indexVm = new FindMembersViewModel();

            string currentUsername = User.Identity.Name != null ? User.Identity.Name : "";

            if (string.IsNullOrEmpty(query))
            {
                indexVm.Users = _userRepository.GetUsers((int)page);
                indexVm.Pages = _pageService.GeneratePages((int)page, _userRepository.GetCount(), 10);
            }
            else
            {
                indexVm.Users = _userRepository.GetUsers(query, (int)page);
                indexVm.Pages = _pageService.GeneratePages((int)page, _userRepository.GetCount(query), 10);
            }

            indexVm.InterestTags = _tagService.GetUserTags(currentUsername);
            indexVm.SearchTags = _tagService.GetAllTags();            
            indexVm.RecommendUsers = _userRepository.GetRecommendUsers(currentUsername);

            return View(indexVm);
        }
    }
}
