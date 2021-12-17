using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.User
{
    public class UserDto
    {
        public Guid Guid { get; set; }
        public string UserName {  get; set; }
        public string UserAvatarImageName { get; set; }
        public ICollection<TagDto> Tags { get; set; }
    }
}
