using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.User
{
    public class UserFollowingDto
    {
        public string Guid { get; set; }
        public string Name {  get; set; }
        public string AvatarImageName { get; set; }
    }
}
