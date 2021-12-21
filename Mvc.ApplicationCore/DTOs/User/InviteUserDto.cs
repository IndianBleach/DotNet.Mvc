using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.User
{
    public class InviteUserDto
    {
        public string InvitedUserGuid {  get; set; }
        public string InvitedToIdeaName { get; set; }
        public string Description { get; set; }
    }
}
