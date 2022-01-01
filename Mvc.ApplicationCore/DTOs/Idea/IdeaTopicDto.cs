using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.Idea
{
    public class IdeaTopicDto
    {
        public string Guid { get; set; }
        public string Title { get; set; }
        public string AuthorGuid { get; set; }
        public string AuthorAvatarImageName { get; set; }
        public string Description {  get; set; }
        public int CommentsCount { get; set; }
    }
}
