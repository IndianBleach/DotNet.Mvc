using Microsoft.AspNetCore.Mvc;
using Mvc.ApplicationCore.DTOs.JsonResult;
using Mvc.ApplicationCore.Interfaces;

namespace Mvc.WebUi.Controllers
{
    public class LoadController : Controller
    {
        private IUserRepository _userRepository;

        public LoadController(IUserRepository userRepo)
        {
            _userRepository = userRepo;
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
