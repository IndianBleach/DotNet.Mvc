using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.Idea
{

    public class TopicCommentDto
    { 
        public string AuthorGuid { get; set; }
        public string AuthorAvatarImage { get; set; }
        public string AuthorName {  get; set; }
        public string DateCreated { get; set; }
        public string Comment { get; set; }

        public TopicCommentDto()
        {
        }

        public TopicCommentDto(string authorGuid,
            string avatar,
            string name,
            string dateCreated,
            string comment)
        {
            AuthorGuid = authorGuid;
            AuthorAvatarImage = avatar;
            AuthorName = name;
            DateCreated = dateCreated;
            Comment = comment;
        }
    }

    public class TopicDetailDto
    {
        public string Guid { get; set; }
        public string AuthorAvatarImage { get; set; }
        public string AuthorGuid { get; set; }
        public string DatePublished { get; set; }
        public string IdeaAvatarImage { get; set; }
        public string Title { get; set; }
        public string Description {  get; set; }
        public List<TopicCommentDto> Comments { get; set; }
    }
}
