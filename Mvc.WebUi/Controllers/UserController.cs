using Microsoft.AspNetCore.Mvc;
using Mvc.ApplicationCore.Interfaces;

namespace Mvc.WebUi.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepo)
        {
            _userRepository = userRepo;
        }
            

        public IActionResult Index()
        {
            return View();
        }
    }
}
