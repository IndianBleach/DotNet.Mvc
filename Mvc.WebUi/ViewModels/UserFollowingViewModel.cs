using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.DTOs.User;

namespace Mvc.WebUi.ViewModels
{
    public class UserFollowingViewModel
    {
        public bool IsFollowed { get; set; }
        public bool IsSelfProfile { get; set; }
        public List<PageInfoDto> Pages { get; set; }
        public List<UserFollowingDto> UserFollowing { get; set; }
        public UserDetailDto User { get; set; }
        public ICollection<string> IdeasToInvite { get; set; }
    }
}
