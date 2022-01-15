using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.DTOs.Idea;

namespace Mvc.WebUi.ViewModels
{
    public class IdeaJoiningsViewModel
    {
        public IdeaDetailDto Idea { get; set; }
        public List<SideIdeaDto> SimilarIdeas { get; set; }
        public List<JoinRequestDto> IdeaJoinings { get; set; }
        public IdeaMemberRoleDto CurrentUserRole { get; set; }
        public List<PageInfoDto> Pages { get; set; }
    }
}
