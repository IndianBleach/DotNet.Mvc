using Microsoft.AspNetCore.Mvc;
using Mvc.ApplicationCore.Interfaces;

namespace Mvc.WebUi.Controllers
{
    public class FindController : Controller
    {
        private readonly ITagService _tagService;

        public FindController(ITagService tagService)
        {
            _tagService= tagService;
            ViewBag.Tags = _tagService.GetAllTags();
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
