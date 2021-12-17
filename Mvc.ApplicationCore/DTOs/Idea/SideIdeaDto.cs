using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.Idea
{
    public class SideIdeaDto
    {
        public Guid Guid { get; set; }
        public string Title { get; set; }
        public string AvatarImageName { get; set; }
    }
}
