using Microsoft.AspNetCore.Mvc;
using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.Interfaces;

namespace Mvc.WebUi.Controllers
{
    public class CreateController : Controller
    {
        private readonly IIdeaRepository _ideaRepository;

        public CreateController(IIdeaRepository ideaRepo)
        {
            _ideaRepository = ideaRepo;
        }

        [Route("jscreate/idea")]
        public async Task<JsonResult> JsIdea(CreateIdeaDto idea)
        {
            string? resGuid = _ideaRepository.CreateIdea(idea);
            if (resGuid != null)
            {
                _ideaRepository.Save();

                return Json(resGuid);
            }

            return Json(null);
        }

        public async Task<IActionResult> Idea(CreateIdeaDto idea)
        {
            string? resGuid = _ideaRepository.CreateIdea(idea);
            if (resGuid != null)
            {
                _ideaRepository.Save();

                return RedirectToAction(resGuid, "idea");
            }

            return BadRequest();
        }
    }
}
