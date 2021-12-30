using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mvc.ApplicationCore.DTOs.Chat;
using Mvc.ApplicationCore.Entities;
using Mvc.ApplicationCore.Identity;
using Mvc.ApplicationCore.Interfaces;
using Mvc.Infrastructure.Data;
using Mvc.WebUi.ViewModels;

namespace Mvc.WebUi.Controllers
{
    public class ChatController : Controller
    {
        private readonly IUserRepository _userRepository;
        private UserManager<ApplicationUser> _userManager;
        private ApplicationContext _dbContext;
        



        public ChatController(IUserRepository userRepo, UserManager<ApplicationUser> userMan, ApplicationContext ctx)
        {
            _userRepository = userRepo;
            _userManager = userMan;
            _dbContext = ctx;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {            
            string guid = await _userRepository.GetUserGuid(User.Identity.Name);
            
            ChatViewModel indexVm = new ChatViewModel();
            indexVm.UserChats = await _userRepository.GetUserChats(guid);
            indexVm.IdeasToInvite = await _userRepository.GetUserIdeasToInvite(guid);

            return View(indexVm);
        }

        [Authorize]
        public async Task<JsonResult> SendMessage(string chatGuid, string message)
        {
            var authorGuid = await _userRepository.GetUserGuid(User.Identity.Name);

            var res = await _userRepository.SendChatMessage(chatGuid, message, authorGuid);

            return Json(res);
        }


        [Authorize]
        public async Task<JsonResult> Create(string toUserGuid, string message)
        {
            
            string authorGuid = await _userRepository.GetUserGuid(User.Identity.Name);

            var res = await _userRepository.CreateChat(authorGuid, message, toUserGuid);

            return Json(res);
        }


        public async Task<JsonResult> GetDetail(string chatGuid)
        {
            var res = await _userRepository.GetChatDetail(chatGuid, User.Identity.Name);

            return Json(res);
        }


        [Authorize]
        public async Task<JsonResult> Detail(string chatGuid)
        {
            var res = await _userRepository.GetChatMessages(chatGuid, User.Identity.Name);

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
