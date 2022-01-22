using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.Interfaces;
using CoreInterfaces = Mvc.ApplicationCore.Interfaces;

namespace Mvc.WebUi.Controllers
{
    public class AccountController : Controller
    {
        private readonly CoreInterfaces.IAuthorizationService _authorizationService;
        private readonly IUserRepository _userRepository;
        private readonly ITagService _tagService;

        public AccountController(CoreInterfaces.IAuthorizationService authServ, ITagService tagService, IUserRepository userRepository)
        {
            _authorizationService = authServ;
            _userRepository = userRepository;
            _tagService = tagService;
        }

        [Authorize]
        public async Task<IActionResult> Remove(string confirmPassword)
        {
            string name = User.Identity.Name;
            if (name != null)
            {
                string guid = await _userRepository.GetUserGuid(name);
            }

            return RedirectToAction("Login");
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _authorizationService.LogoutAsync();

            return RedirectToAction("index", "home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto userModel)
        {
            if (ModelState.IsValid)
            {
                await _authorizationService.LoginAsync(userModel);

                return RedirectToAction("index", "home");
            }
            else
                return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDto userModel)
        {
            if (ModelState.IsValid)
            {
                await _authorizationService.RegisterAsync(userModel);

                return RedirectToAction("index", "home");
            }
            else
            {
                ViewBag.TagList = _tagService.GetAllTags();
                return View(userModel);
            }    
        }

        [HttpGet]
        public IActionResult Login()
            => View();

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.TagList = _tagService.GetAllTags();

            return View();
        }
    }
}
