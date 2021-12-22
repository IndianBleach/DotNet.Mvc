using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.Chat
{
    public class ChatUserDto
    {
        public string UserGuid { get; set; }
        public string UserName { get; set; }
        public string AvatarImageName { get; set; }
        public string? LastMessage { get; set; }
    }
}
