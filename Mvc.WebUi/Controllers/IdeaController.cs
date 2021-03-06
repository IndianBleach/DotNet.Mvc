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
        private readonly IPageService _pageService;
        private readonly ApplicationContext _dbContext;

        public IdeaController(IIdeaRepository ideaRepository, IUserRepository userRepository, ApplicationContext ctx, IPageService pageService)
        {
            _ideaRepository = ideaRepository;
            _userRepository = userRepository;
            _dbContext = ctx;
            _pageService = pageService;
        }

        [Authorize]
        [HttpPost]
        [Route("idea/acceptJoin")]
        public async Task<JsonResult> AcceptJoin(string joinGuid)
        {
            //var guid = await _userRepository.GetUserGuid(User.Identity.Name);

            var res = await _ideaRepository.AcceptJoinAsync(joinGuid);

            _userRepository.Save();

            return Json(res);
        }


        [Authorize]
        [HttpPost]
        [Route("idea/declineJoin")]
        public async Task<JsonResult> DeclineJoin(string joinGuid)
        {
            //var guid = await _userRepository.GetUserGuid(User.Identity.Name);

            var res = await _ideaRepository.DeclineJoinAsync(joinGuid);

            _userRepository.Save();

            return Json(res);
        }


        [Authorize]
        [HttpPost]
        [Route("idea/removeIdea")]
        public async Task<IActionResult> RemoveIdea(string confirmPassword, string ideaGuid)
        {
            string userGuid = await _userRepository.GetUserGuid(User.Identity.Name);
            bool res = await _ideaRepository.RemoveIdeaAsync(ideaGuid, confirmPassword, userGuid);

            _ideaRepository.Save();

            return RedirectToAction("Index", "home");
        }


        [Authorize]
        [HttpPost]
        [Route("idea/updateIdeaSettings")]
        public async Task<IActionResult> UpdateIdeaSettings(IFormFile avatar, string description, string ideaGuid, int status, bool isSecutiry)
        {
            string userGuid = await _userRepository.GetUserGuid(User.Identity.Name);

            var res = await _ideaRepository.UpdateIdeaSettingsAsync(avatar, (IdeaStatuses)status, description, isSecutiry, ideaGuid, userGuid);

            _ideaRepository.Save();

            return RedirectToAction(res.ToString(), "idea");
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
        public async Task<IActionResult> CreateTopic(string title, string description, string ideaGuid)
        {
            string authorGuid = await _userRepository.GetUserGuid(User.Identity.Name);
            bool res = await _ideaRepository.CreateTopic(title, description, authorGuid, ideaGuid);

            if (res) _ideaRepository.Save();

            return RedirectToAction(ideaGuid);
        }


        [HttpGet]
        [Route("idea/{ideaGuid}")]
        public async Task<IActionResult> Index(string ideaGuid, string? section, int? page)
        {
            if (page == null)
                page = 1;

            string currentUserName = User.Identity.Name;

            if (section == "goals")
            {
                IdeaBoxesViewModel goalsVm = new IdeaBoxesViewModel();
                goalsVm.Idea = _ideaRepository.GetIdeaDetail(ideaGuid);
                goalsVm.SimilarIdeas = _ideaRepository.GetSimilarIdeas(ideaGuid);
                goalsVm.IdeaBoxes = await _ideaRepository.GetIdeaBoxesAsync(ideaGuid, await _userRepository.GetUserGuid(currentUserName), (int)page);
                goalsVm.CurrentUserRole = IdeaMemberRoleDto.Viewer;
                goalsVm.Pages = _pageService.GeneratePages((int)page, goalsVm.IdeaBoxes.Count, 10);

                if (currentUserName != null)
                    goalsVm.CurrentUserRole = _ideaRepository.GetIdeaMemberRole(ideaGuid,
                        await _userRepository.GetUserGuid(currentUserName));

                return View("Boxes", goalsVm);
            }
            else if (section == "members")
            {
                IdeaMembersViewModel membersVm = new IdeaMembersViewModel();
                membersVm.Idea = _ideaRepository.GetIdeaDetail(ideaGuid);
                membersVm.SimilarIdeas = _ideaRepository.GetSimilarIdeas(ideaGuid);
                membersVm.IdeaMembers = await _ideaRepository.GetIdeaMembersAsync(ideaGuid, (int)page);
                membersVm.CurrentUserRole = IdeaMemberRoleDto.Viewer;
                membersVm.Pages = _pageService.GeneratePages((int)page, membersVm.IdeaMembers.Count, 10);

                if (currentUserName != null)
                    membersVm.CurrentUserRole = _ideaRepository.GetIdeaMemberRole(ideaGuid,
                        await _userRepository.GetUserGuid(currentUserName));

                return View("Members", membersVm);
            }
            else if (section == "joinings")
            {
                IdeaJoiningsViewModel joiningsVm = new IdeaJoiningsViewModel();
                joiningsVm.Idea = _ideaRepository.GetIdeaDetail(ideaGuid);
                joiningsVm.SimilarIdeas = _ideaRepository.GetSimilarIdeas(ideaGuid);
                joiningsVm.IdeaJoinings = await _ideaRepository.GetIdeaJoinRequests(ideaGuid, (int)page);
                joiningsVm.CurrentUserRole = IdeaMemberRoleDto.Viewer;
                joiningsVm.Pages = _pageService.GeneratePages((int)page, joiningsVm.IdeaJoinings.Count, 10);

                if (currentUserName != null)
                    joiningsVm.CurrentUserRole = _ideaRepository.GetIdeaMemberRole(ideaGuid,
                        await _userRepository.GetUserGuid(currentUserName));

                return View("Joinings", joiningsVm);
            }
            
            IdeaAboutViewModel indexVm = new IdeaAboutViewModel();
            indexVm.Idea = _ideaRepository.GetIdeaDetail(ideaGuid);
            indexVm.SimilarIdeas = _ideaRepository.GetSimilarIdeas(ideaGuid);
            indexVm.IdeaTopics = await _ideaRepository.GetIdeaTopicsAsync(ideaGuid, (int)page);
            indexVm.CurrentUserRole = IdeaMemberRoleDto.Viewer;
            indexVm.Pages = _pageService.GeneratePages((int)page, indexVm.IdeaTopics.Count, 10);

            if (currentUserName != null)
                indexVm.CurrentUserRole = _ideaRepository.GetIdeaMemberRole(ideaGuid,
                    await _userRepository.GetUserGuid(currentUserName));

            return View(indexVm);
        }
    }
}
