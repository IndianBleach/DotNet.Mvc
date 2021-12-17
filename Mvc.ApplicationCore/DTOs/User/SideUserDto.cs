using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.User
{
    public class SideUserDto
    {
        public string AvatarImageName { get; set; }
        public Guid Guid { get; set; }
        public string UserName {  get; set; }
    }
}
