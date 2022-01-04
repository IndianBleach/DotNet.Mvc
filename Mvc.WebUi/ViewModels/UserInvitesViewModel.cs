using Mvc.ApplicationCore.DTOs.User;

namespace Mvc.WebUi.ViewModels
{
    public class UserInvitesViewModel
    {
        public bool IsFollowed { get; set; }
        public bool IsSelfProfile { get; set; }
        //public List<PageInfoDto> Pages { get; set; }
        public List<InviteDetailDto> UserInvites { get; set; }
        public UserDetailDto User { get; set; }
        public ICollection<string> IdeasToInvite { get; set; }
    }
}
