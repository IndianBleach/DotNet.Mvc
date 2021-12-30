using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.Chat
{
    public class ChatDetailDto
    {
        public ChatAuthorInfoDto AuthorInfo { get; set; }
        public ICollection<MessageDetailDto> Messages { get; set; }
    }
}
