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
        private readonly ITagService _tagService;

        public AccountController(CoreInterfaces.IAuthorizationService authServ, ITagService tagService)
        {
            _authorizationService = authServ;
            _tagService = tagService;
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
            await _authorizationService.LoginAsync(userModel);

            return RedirectToAction("index", "home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDto userModel)
        {
            await _authorizationService.RegisterAsync(userModel);

            return RedirectToAction("index", "home");
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
