using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mvc.ApplicationCore.DTOs.Idea;
using Mvc.ApplicationCore.Interfaces;
using Mvc.WebUi.ViewModels;

namespace Mvc.WebUi.Controllers
{
    public class IdeaController : Controller
    {
        private readonly IIdeaRepository _ideaRepository;
        private readonly IUserRepository _userRepository;

        public IdeaController(IIdeaRepository ideaRepository, IUserRepository userRepository)
        {
            _ideaRepository = ideaRepository;
            _userRepository = userRepository;
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
