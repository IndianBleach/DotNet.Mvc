using Microsoft.AspNetCore.Mvc;
using Mvc.ApplicationCore.Interfaces;
using Mvc.WebUi.ViewModels;

namespace Mvc.WebUi.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPageService _pageService;

        public UserController(IUserRepository userRepo, IPageService pageService)
        {
            _userRepository = userRepo;
            _pageService = pageService;
        }

        [Route("user/{guid}")]
        [HttpGet]
        public IActionResult Index(string guid, int? page)
        {
            if (page == null) page = 1;

            UserProfileViewModel indexVm = new UserProfileViewModel();
            indexVm.User = _userRepository.GetUserDetail(guid);
            indexVm.UserIdeas = _userRepository.GetUserIdeas(guid);
            indexVm.Pages = _pageService.GeneratePages((int)page, _userRepository.GetUserIdeasCount(guid), 5);

            return View(indexVm);
        }
    }
}
