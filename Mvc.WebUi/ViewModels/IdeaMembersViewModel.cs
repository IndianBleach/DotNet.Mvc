using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.DTOs.Idea;

namespace Mvc.WebUi.ViewModels
{
    public class IdeaMembersViewModel
    {
        public IdeaDetailDto Idea { get; set; }
        public List<SideIdeaDto> SimilarIdeas { get; set; }
        public List<IdeaMemberDto> IdeaMembers { get; set; }
        public IdeaMemberRoleDto CurrentUserRole { get; set; }
        public List<PageInfoDto> Pages { get; set; }
    }
}
