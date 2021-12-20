using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.DTOs.User;

namespace Mvc.WebUi.ViewModels
{
    public class UserEditProfileViewModel
    {
        public List<TagDto> TagList { get; set; }
        public UserDetailDto User { get; set; }
    }
}
