using Mvc.ApplicationCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Entities.IdeaEntity
{
    public class IdeaTopic : BaseEntity
    {
        public Guid Guid { get; set; }
        public int AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public int IdeaId { get; set; }
        public Idea Idea { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<CommentMessage> Comments { get; set; }

        public IdeaTopic(
            string title,
            string description,
            int ideaId,
            int authorId)
        {
            Title = title;
            Description = description;
            IdeaId = ideaId;
            AuthorId = authorId;
            Guid = Guid.NewGuid();
            DateCreated = DateTime.Now;
            Comments = new List<CommentMessage>();
        }

        public IdeaTopic(
            ApplicationUser author,
            Idea idea,
            string title,
            string description)
        {
            Author = author;
            Idea = idea;
            Title = title;
            Description = description;
            Guid = Guid.NewGuid();
            DateCreated = DateTime.Now;
            Comments = new List<CommentMessage>();
        }
    }
}
