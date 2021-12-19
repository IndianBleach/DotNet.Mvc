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

        public async Task<IActionResult> Idea(CreateIdeaDto idea)
        {
            var res = _ideaRepository.CreateIdea(idea);
            _ideaRepository.Save();

            return RedirectToAction("index", "home");
        }
    }
}
