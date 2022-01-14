using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.Idea
{
    public class JoinRequestDto
    {
        public string InviteGuid { get; set; }
        public string FromUserGuid { get; set; }
        public string FromUserAvatar { get; set; }
        public string FromUserName { get; set; }
        public string Description { get; set; }
    }
}
