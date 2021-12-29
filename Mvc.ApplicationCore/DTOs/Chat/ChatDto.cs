using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.Chat
{
    public class ChatDto
    {
        public string Guid { get; set; }
        public List<MessageDetailDto> Messages { get; set; }

    }
}
