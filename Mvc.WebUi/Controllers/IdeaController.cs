using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mvc.ApplicationCore.DTOs.Idea;
using Mvc.ApplicationCore.Entities.IdeaEntity;
using Mvc.ApplicationCore.Interfaces;
using Mvc.Infrastructure.Data;
using Mvc.WebUi.ViewModels;

namespace Mvc.WebUi.Controllers
{
    public class IdeaController : Controller
    {
        private readonly IIdeaRepository _ideaRepository;
        private readonly IUserRepository _userRepository;
        private readonly ApplicationContext _dbContext;

        public IdeaController(IIdeaRepository ideaRepository, IUserRepository userRepository, ApplicationContext ctx)
        {
            _ideaRepository = ideaRepository;
            _userRepository = userRepository;
            _dbContext = ctx;
        }

        [Authorize]
        [HttpPost]
        [Route("idea/cickMember")]
        public async Task<JsonResult> CickMember(string roleGuid)
        {
            var res = await _ideaRepository.CickMember(roleGuid);

            _ideaRepository.Save();

            return Json(res);
        }

        [Authorize]
        [HttpPost]
        [Route("idea/addModder")]
        public async Task<JsonResult> AddModder(string roleGuid)
        {
            var res = await _ideaRepository.AddModder(roleGuid);

            _ideaRepository.Save();

            return Json(res);
        }

        [Authorize]
        [HttpPost]
        [Route("idea/removeModder")]
        public async Task<JsonResult> RemoveModder(string roleGuid)
        {
            var res = await _ideaRepository.RemoveModder(roleGuid);

            _ideaRepository.Save();

            return Json(res);
        }


        [Authorize]
        [HttpPost]
        [Route("idea/createTopic")]
        public async Task<IActionResult> NewTopic(string title, string description, string ideaGuid)
        {
            string authorGuid = await _userRepository.GetUserGuid(User.Identity.Name);
            bool res = await _ideaRepository.CreateTopic(title, description, authorGuid, ideaGuid);

            if (res) _ideaRepository.Save();

            string currentUserName = User.Identity.Name;

            IdeaAboutViewModel indexVm = new IdeaAboutViewModel();
            indexVm.Idea = _ideaRepository.GetIdeaDetail(ideaGuid);
            indexVm.SimilarIdeas = new List<SideIdeaDto>();
            indexVm.IdeaTopics = _ideaRepository.GetIdeaTopics(ideaGuid);
            indexVm.CurrentUserRole = IdeaMemberRoleDto.Viewer;

            if (currentUserName != null)
                indexVm.CurrentUserRole = _ideaRepository.GetIdeaMemberRole(ideaGuid,
                    await _userRepository.GetUserGuid(currentUserName));

            return View("Index", indexVm);
        }


        
        [Route("idea/{ideaGuid}")]
        public async Task<IActionResult> Index(string ideaGuid)
        {
            string currentUserName = User.Identity.Name;

            IdeaAboutViewModel indexVm = new IdeaAboutViewModel();
            indexVm.Idea = _ideaRepository.GetIdeaDetail(ideaGuid);
            indexVm.SimilarIdeas = new List<SideIdeaDto>();
            indexVm.IdeaTopics = _ideaRepository.GetIdeaTopics(ideaGuid);
            indexVm.CurrentUserRole = IdeaMemberRoleDto.Viewer;

            if (currentUserName != null)
                indexVm.CurrentUserRole = _ideaRepository.GetIdeaMemberRole(ideaGuid, 
                    await _userRepository.GetUserGuid(currentUserName));

            return View(indexVm);
        }
    }
}
