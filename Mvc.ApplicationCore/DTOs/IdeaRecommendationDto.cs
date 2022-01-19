using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.Idea
{
    public class IdeaRecommendationDto
    {
        public Guid IdeaGuid { get; set; }
        public string AuthorGuid { get; set; }
        public string IdeaTitle { get; set; }
        public string AuthorName { get; set; }        
        public string UserAvatarImageName { get; set; }
    }
}
