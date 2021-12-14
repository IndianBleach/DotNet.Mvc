using Microsoft.AspNetCore.Mvc;
using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.Interfaces;

namespace Mvc.WebUi.Components
{
    public class TagListViewComponent : ViewComponent
    {
        private readonly ITagService _tagService;

        public TagListViewComponent(ITagService tagService)
        {
            _tagService = tagService;
        }

        public IViewComponentResult Invoke()
        {
            return View(_tagService.GetAllTags());
        }
    }
}
