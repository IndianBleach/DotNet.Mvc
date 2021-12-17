using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.DTOs.Idea;

namespace Mvc.WebUi.ViewModels
{
    public class HomeIdeasViewModel
    {

        public ICollection<TagDto> SearchTags { get; set; }
        public ICollection<HomeIdeaDto> Ideas { get; set; }

    }
}
