using Mvc.ApplicationCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Entities.IdeaEntity
{
    public class IdeaStar : BaseEntity
    {
        public int AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public int IdeaId { get; set; }
        public Idea Idea { get; set; }

        public IdeaStar(int authorId, int ideaId)
        {
            AuthorId = authorId;
            IdeaId = ideaId;
        }

        public IdeaStar(ApplicationUser user, Idea idea)
        {
            Author = user;
            Idea = idea;
        }
    }
}
