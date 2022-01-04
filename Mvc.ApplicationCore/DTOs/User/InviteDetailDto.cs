using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.User
{
    public class InviteDetailDto
    {
        public string InviteGuid { get; set; }
        public string FromUserGuid { get; set; }
        public string FromUserAvatar { get; set; }
        public string FromUserName { get; set; }
        public string IdeaGuid { get; set; }
        public string IdeaName {  get; set; }
        public string Description { get; set; }
    }
}
