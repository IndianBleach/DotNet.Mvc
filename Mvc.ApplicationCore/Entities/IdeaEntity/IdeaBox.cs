using Mvc.ApplicationCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Entities.IdeaEntity
{
    public class IdeaBox : BaseEntity
    {
        public Guid Guid { get; set; }
        public int AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<CommentMessage> Comments { get; set; }

        public IdeaBox(
            int authorId,
            string title,
            string description,
            bool isConfirmed
            )
        {
            AuthorId = authorId;
            Title = title;
            IsConfirmed = isConfirmed;
            Description = description;
            Guid = Guid.NewGuid();
            DateCreated = DateTime.Now;
            Comments = new List<CommentMessage>();
        }

        public IdeaBox(
            ApplicationUser user,
            string title,
            string description,
            bool isConfirmed
            )
        {
            Author = user;
            Title = title;
            IsConfirmed = isConfirmed;
            Description = description;
            Guid = Guid.NewGuid();
            DateCreated = DateTime.Now;
        }
    }
}
