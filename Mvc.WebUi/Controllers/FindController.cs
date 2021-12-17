using Microsoft.AspNetCore.Mvc;
using Mvc.ApplicationCore.Interfaces;
using Mvc.WebUi.ViewModels;

namespace Mvc.WebUi.Controllers
{
    public class FindController : Controller
    {
        private readonly ITagService _tagService;
        private readonly IUserRepository _userRepository;

        public FindController(ITagService tagService, IUserRepository userRepo)
        {
            _tagService= tagService;
            ViewBag.Tags = _tagService.GetAllTags();
            _userRepository = userRepo;
        }


        [HttpGet]
        public IActionResult Index()
        {
            FindMembersViewModel indexVm = new FindMembersViewModel();

            indexVm.InterestTags = _tagService.GetUserTags(User.Identity.Name);
            indexVm.SearchTags = _tagService.GetAllTags();
            indexVm.Users = _userRepository.GetUsersPerPage(1);
            indexVm.RecommendUsers = _userRepository.GetRecommendUsers(User.Identity.Name);

            return View(indexVm);
        }
    }
}
