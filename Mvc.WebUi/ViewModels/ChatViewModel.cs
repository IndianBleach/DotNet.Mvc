using Mvc.ApplicationCore.DTOs.Chat;

namespace Mvc.WebUi.ViewModels
{
    public class ChatViewModel
    {
        public ICollection<ChatUserDto> UserChats { get; set; }

        public ChatViewModel()
        {
            UserChats = new List<ChatUserDto>();
        }
    }
}
