using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mvc.ApplicationCore.Interfaces;
using Mvc.WebUi.ViewModels;

namespace Mvc.WebUi.Controllers
{
    public class ChatController : Controller
    {
        private readonly IUserRepository _userRepository;

        public ChatController(IUserRepository userRepo)
        {
            _userRepository = userRepo;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            string guid = await _userRepository.GetUserGuid(User.Identity.Name);

            ChatViewModel indexVm = new ChatViewModel();
            indexVm.UserChats = await _userRepository.GetUserChats(guid);

            return View(indexVm);
        }

        public async Task<JsonResult> New(string user)
        { 
            string userGuid = await _userRepository.GetUserGuid(User.Identity.Name);

            var res = await _userRepository.CreateChatWithUser(userGuid, user);

            _userRepository.Save();

            return Json(res);
        }



        public async Task<JsonResult> NewChats()
        {
            var guid = await _userRepository.GetUserGuid(User.Identity.Name);

            var res = await _userRepository.GetNewChatUsersAsync(guid);

            return Json(res);
        }
    }
}
