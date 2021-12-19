using Microsoft.AspNetCore.Mvc;
using Mvc.ApplicationCore.Interfaces;
using Mvc.WebUi.ViewModels;

namespace Mvc.WebUi.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepo)
        {
            _userRepository = userRepo;
        }

        [Route("user/{guid}")]
        [HttpGet]
        public IActionResult Index(string guid)
        {
            UserProfileViewModel indexVm = new UserProfileViewModel();
            indexVm.User = _userRepository.GetUserDetail(guid);
            indexVm.UserIdeas = _userRepository.GetUserIdeas(guid);

            return View(indexVm);
        }
    }
}
