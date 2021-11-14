using Mvc.ApplicationCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Entities
{
    public class CommentMessage : BaseEntity
    {
        public string Message { get; set; }
        public int AuthorId { get; set; }
        public ApplicationUser Author { get; set; }        
        public DateTime DateCreated { get; set; }        

        public CommentMessage(
            string message,
            int authorId)
        {
            Message = message;
            AuthorId = authorId;
            DateCreated = DateTime.Now;
        }
    }
}
