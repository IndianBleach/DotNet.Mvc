using Mvc.ApplicationCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Entities.IdeaEntity
{
    public class TopicComment : BaseEntity
    {
        public string Message { get; set; }
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public DateTime DateCreated { get; set; }        
        public int TopicId { get; set; }
        public IdeaTopic Topic { get; set; }

        public TopicComment(int topicId, string authorId, string message)
        {
            TopicId = topicId;
            AuthorId = authorId;
            Message = message;
            DateCreated = DateTime.Now;
        }
    }
}
