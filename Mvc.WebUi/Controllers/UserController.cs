using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mvc.ApplicationCore.DTOs.JsonResult;
using Mvc.ApplicationCore.DTOs.User;
using Mvc.ApplicationCore.Interfaces;
using Mvc.WebUi.ViewModels;

namespace Mvc.WebUi.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPageService _pageService;
        private readonly ITagService _tagService;

        public UserController(IUserRepository userRepo, IPageService pageService, ITagService tagService)
        {
            _userRepository = userRepo;
            _pageService = pageService;
            _tagService = tagService;
        }

        public async Task<JsonResult> Invite(InviteUserDto model)
        {
            var guid = await _userRepository.GetUserGuid(User.Identity.Name);

            var res = await _userRepository.InviteUserToIdea(guid, model);

            return Json(res);
        }

        


        [Route("user/me")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Me(int? page)
        {
            if (page == null) page = 1;

            string userGuid = await _userRepository.GetUserGuid(User.Identity.Name);

            UserProfileViewModel indexVm = new UserProfileViewModel();
            indexVm.User = _userRepository.GetUserDetail(userGuid);
            indexVm.UserIdeas = _userRepository.GetUserIdeas(userGuid);
            indexVm.Pages = _pageService.GeneratePages((int)page, _userRepository.GetUserIdeasCount(userGuid), 5);
            indexVm.IsSelfProfile = true;

            return View("Index", indexVm);
        }


        [Route("user/{guid}")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(string guid, int? page)
        {
            if (page == null) page = 1;

            UserProfileViewModel indexVm = new UserProfileViewModel();
            indexVm.User = _userRepository.GetUserDetail(guid);
            indexVm.UserIdeas = _userRepository.GetUserIdeas(guid);
            indexVm.Pages = _pageService.GeneratePages((int)page, _userRepository.GetUserIdeasCount(guid), 5);
            indexVm.IsSelfProfile = _userRepository.CheckSelfProfile(guid, User.Identity.Name);
            indexVm.IdeasToInvite = await _userRepository.GetIdeasToInvite(User.Identity.Name);

            return View(indexVm);
        }


        [Route("user/edit")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            UserEditProfileViewModel indexVm = new UserEditProfileViewModel();

            var userId = User.Identity.Name;

            var guid = await _userRepository.GetUserGuid(userId);

            indexVm.TagList = _tagService.GetAllTags().ToList();
            indexVm.User = _userRepository.GetUserDetail(guid);            

            return View(indexVm);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateGeneralSettings(UserEditGeneralSettingsDto model)
        {
            var res1 = User.Identity.Name;

            bool res = await _userRepository.UpdateUserSettings(User.Identity.Name, model);

            if (res)
            {
                return RedirectToAction("edit", "user");
            }

            return Content("Something missing");
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateTagSettings(UserEditTagSettingsDto model)
        {
            bool res = await _userRepository.UpdateUserSettings(User.Identity.Name, model);

            if (res)
            {
                return RedirectToAction("edit", "user");
            }

            return Content("Something missing");
        }

        
    }
}
