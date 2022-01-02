using Microsoft.AspNetCore.Mvc;
using Mvc.ApplicationCore.DTOs.JsonResult;
using Mvc.ApplicationCore.Interfaces;

namespace Mvc.WebUi.Controllers
{
    public class LoadController : Controller
    {
        private IUserRepository _userRepository;
        private IIdeaRepository _ideaRepository;

        public LoadController(IUserRepository userRepo, IIdeaRepository ideaRepository)
        {
            _userRepository = userRepo;
            _ideaRepository = ideaRepository;
        }


        [HttpGet]
        public async Task<JsonResult> IdeaRoles(string ideaGuid)
        {
            var res = await _ideaRepository.GetIdeaRolesAsync(ideaGuid);

            return Json(res);
        }


        //[HttpPost]
        public async Task<JsonResult> LoadParticipation(string userName)
        {
            string guid = await _userRepository.GetUserGuid(userName);

            List<UserParticipationDto> res = _userRepository.GetUserParticipations(userName);

            return Json(res);
        }
    }
}
