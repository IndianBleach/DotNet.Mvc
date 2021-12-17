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
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public int IdeaId { get; set; }
        public Idea Idea { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<CommentMessage> Comments { get; set; }
        public bool IsDefault { get; set; }

        public IdeaTopic(string title, string description, ApplicationUser author, bool isDefault)
        {
            Title = title;
            Description = description;
            Author = author;
            Comments = new List<CommentMessage>();
            DateCreated = DateTime.Now;
            Guid = Guid.NewGuid();
            IsDefault = isDefault;
            AuthorId = author.Id;
        }

        public IdeaTopic(string title, string description, string authorId)
        {
            Title = title;
            Description = description;
            AuthorId = authorId;
            Comments = new List<CommentMessage>();
            DateCreated = DateTime.Now;
            Guid = Guid.NewGuid();
        }

        public IdeaTopic(
            string title,
            string description,
            int ideaId,
            string authorId)
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
