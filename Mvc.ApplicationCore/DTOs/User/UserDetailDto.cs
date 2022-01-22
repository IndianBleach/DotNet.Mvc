using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.User
{
    public class UserDetailDto
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<TagDto> Tags { get; set; }
        public string AvatarImageName { get; set; }
        public int CountSubscribers { get; set; }
        public int CountFollowing {  get; set; }
        public string DateCreated { get; set; }
    }
}
