using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.Idea
{
    public class RecommendIdeaDto
    {
        public Guid Guid { get; set; }
        public string AuthorName { get; set; }        
        public string AvatarImageName { get; set; }
        public string Title { get; set; }
    }
}
