using Mvc.ApplicationCore.DTOs.Idea;

namespace Mvc.WebUi.ViewModels
{
    public class IdeaBoxesViewModel
    {
        public IdeaDetailDto Idea { get; set; }
        public List<SideIdeaDto> SimilarIdeas { get; set; }
        public List<IdeaBoxDto> IdeaBoxes { get; set; }
        public IdeaMemberRoleDto CurrentUserRole { get; set; }
    }
}
