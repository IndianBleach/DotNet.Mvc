using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs
{
    public class CreateCommentDto
    {
        public string CommentMessage { get; set; }
        public int AuthorId { get; set; }
        public int IdeaId { get; set; }

        public CreateCommentDto(
            string comment,
            int authorId,
            int ideaId)
        {
            CommentMessage = comment;
            AuthorId = authorId;
            IdeaId = ideaId;
        }
    }
}
