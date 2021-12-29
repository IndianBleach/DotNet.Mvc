using Mvc.ApplicationCore.DTOs.Chat;
using Mvc.ApplicationCore.DTOs.Idea;

namespace Mvc.WebUi.ViewModels
{
    public class ChatViewModel
    {
        public ICollection<ChatUserDto> UserChats { get; set; }
        public ICollection<IdeaToInviteDto> IdeasToInvite { get; set; }

        public ChatViewModel()
        {
            UserChats = new List<ChatUserDto>();
        }
    }
}
