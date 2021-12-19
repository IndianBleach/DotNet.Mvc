using Mvc.ApplicationCore.DTOs.Idea;
using Mvc.ApplicationCore.DTOs.User;

namespace Mvc.WebUi.ViewModels
{
    public class UserProfileViewModel
    {
        public ICollection<HomeIdeaDto> UserIdeas { get; set; }
        public UserDetailDto User { get; set; }
    }
}
