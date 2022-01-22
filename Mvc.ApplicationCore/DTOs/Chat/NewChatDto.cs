using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.Chat
{
    public class NewChatDto
    {
        public string AvatarImageName {  get; set; }
        public string UserName { get; set; }
        public string UserGuid { get; set; }
    }

    public class NewChatDtoExtended
    { 
        public List<NewChatDto> NewChatUsers { get; set; }
        public string AuthorGuid { get; set; }
    }

}
