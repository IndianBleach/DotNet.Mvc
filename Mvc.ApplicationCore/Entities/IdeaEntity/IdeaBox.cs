using Mvc.ApplicationCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Entities.IdeaEntity
{
    public class IdeaBoxGoal : BaseEntity
    {
        public string GoalContent { get; set; }
        public int AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public DateTime DateCreated { get; set; }
        public int IdeaBoxId { get; set; }
        public IdeaBox IdeaBox { get; set; }

        public IdeaBoxGoal(string goalContent, ApplicationUser author, IdeaBox ideaBox)
        {
            DateCreated = DateTime.Now;
            Author = author;
            IdeaBox = ideaBox;
            GoalContent = goalContent;
        }

        public IdeaBoxGoal(string goalContent, int authorId, int ideaBoxId)
        {
            GoalContent = goalContent;
            AuthorId = authorId;
            IdeaBoxId = ideaBoxId;
            DateCreated = DateTime.Now;
        }
    }


    public class IdeaBox : BaseEntity
    {
        public Guid Guid { get; set; }
        public int AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<IdeaBoxGoal> Goals { get; set; }
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
            Goals = new List<IdeaBoxGoal>();
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
            Goals = new List<IdeaBoxGoal>();
        }
    }
}
