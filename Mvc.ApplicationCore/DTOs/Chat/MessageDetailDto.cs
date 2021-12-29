using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.Chat
{
    public class MessageDetailDto
    {
        public string AuthorName { get; set; }
        public string AvatarImageName { get; set; }
        public string Message { get; set; }
        public string DateCreated { get; set; } 
        public bool IsAuthorMessage { get; set; }
    }
}
