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
        [Route("idea/goals/update")]
        public async Task<JsonResult> UpdateGoalStatus(string goalGuid, int goalStatus)
        {
            string userGuid = await _userRepository.GetUserGuid(User.Identity.Name);
            var res = await _ideaRepository.UpdateGoalStatusAsync(goalGuid, (BoxGoalStatuses)goalStatus, userGuid);

            _ideaRepository.Save();

            return Json(res);
        }

        [Authorize]
        [HttpPost]
        [Route("idea/goals/remove")]
        public async Task<JsonResult> RemoveGoal(string goalGuid)
        {
            string userGuid = await _userRepository.GetUserGuid(User.Identity.Name);
            var res = await _ideaRepository.RemoveGoalAsync(goalGuid, userGuid);

            _ideaRepository.Save();

            return Json(res);
        }



        [Authorize]
        [HttpPost]
        [Route("idea/boxes/creategoal")]
        public async Task<JsonResult> CreateBoxGoal(string boxGuid, string content)
        {
            string authorGuid = await _userRepository.GetUserGuid(User.Identity.Name);

            var res = await _ideaRepository.CreateBoxGoalAsync(boxGuid, authorGuid, content);

            _ideaRepository.Save();

            return Json(res);
        }


        [Authorize]
        [HttpGet]
        [Route("idea/boxes/detail")]
        public async Task<JsonResult> BoxDetail(string boxGuid)
        {
            string authorGuid = await _userRepository.GetUserGuid(User.Identity.Name);

            var res = await _ideaRepository.GetBoxDetailAsync(boxGuid, authorGuid);

            return Json(res);
        }


        [Authorize]
        [HttpPost]
        [Route("idea/createBox")]
        public async Task<IActionResult> CreateBox(string title, string description, string ideaGuid, bool isAuthored)
        {
            string authorGuid = await _userRepository.GetUserGuid(User.Identity.Name);

            var res = await _ideaRepository.CreateBoxAsync(title, description, isAuthored, authorGuid, ideaGuid);

            if (res) _ideaRepository.Save();           

            return RedirectToAction(ideaGuid, new {section = "goals"});
        }



        [Authorize]
        [HttpPost]
        [Route("idea/topics/createcomment")]
        public async Task<JsonResult> CreateTopicComment(string topicGuid, string message)
        {
            string authorGuid = await _userRepository.GetUserGuid(User.Identity.Name);

            var res = await _ideaRepository.CreateTopicComment(topicGuid, authorGuid, message);

            _ideaRepository.Save();

            return Json(res);
        }


        [Authorize]
        [HttpGet]
        [Route("idea/topicdetail")]
        public async Task<JsonResult> TopicDetail(string topicGuid)
        {
            var res = await _ideaRepository.GetTopicDetail(topicGuid);

            return Json(res);
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
        public async Task<IActionResult> Index(string ideaGuid, string? section)
        {
            string currentUserName = User.Identity.Name;

            if (section == "goals")
            {
                IdeaBoxesViewModel goalsVm = new IdeaBoxesViewModel();
                goalsVm.Idea = _ideaRepository.GetIdeaDetail(ideaGuid);
                goalsVm.SimilarIdeas = new List<SideIdeaDto>();
                goalsVm.IdeaBoxes = await _ideaRepository.GetIdeaBoxesAsync(ideaGuid, await _userRepository.GetUserGuid(currentUserName));
                goalsVm.CurrentUserRole = IdeaMemberRoleDto.Viewer;

                if (currentUserName != null)
                    goalsVm.CurrentUserRole = _ideaRepository.GetIdeaMemberRole(ideaGuid,
                        await _userRepository.GetUserGuid(currentUserName));

                return View("Boxes", goalsVm);
            }
            else if (section == "members")
            {
                IdeaMembersViewModel membersVm = new IdeaMembersViewModel();
                membersVm.Idea = _ideaRepository.GetIdeaDetail(ideaGuid);
                membersVm.SimilarIdeas = new List<SideIdeaDto>();
                membersVm.IdeaMembers = await _ideaRepository.GetIdeaMembersAsync(ideaGuid);
                membersVm.CurrentUserRole = IdeaMemberRoleDto.Viewer;

                if (currentUserName != null)
                    membersVm.CurrentUserRole = _ideaRepository.GetIdeaMemberRole(ideaGuid,
                        await _userRepository.GetUserGuid(currentUserName));

                return View("Members", membersVm);
            }
            
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
