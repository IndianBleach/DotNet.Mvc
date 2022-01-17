using Microsoft.AspNetCore.Mvc;

namespace Mvc.WebUi.Components
{
    public class IdeaMenuViewComponent : ViewComponent
    {
        public IdeaMenuViewComponent()
        {
        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
