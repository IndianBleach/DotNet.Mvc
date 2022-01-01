using Mvc.ApplicationCore.DTOs.Idea;
using Mvc.ApplicationCore.Entities.IdeaEntity;

namespace Mvc.WebUi.ViewModels
{
    public class IdeaAboutViewModel
    {
        public IdeaDetailDto Idea { get; set; }
        public List<SideIdeaDto> SimilarIdeas { get; set; }
        public List<IdeaTopicDto> IdeaTopics { get; set; } 
        public IdeaMemberRoleDto CurrentUserRole { get; set; }
    }
}
