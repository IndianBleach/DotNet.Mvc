using Mvc.ApplicationCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Entities.IdeaEntity
{
    public class IdeaBoxLike : BaseEntity
    {
        public int AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public int BoxId { get; set; }
        public IdeaBox Box { get; set; }

        public IdeaBoxLike(int authorId, int boxId)
        {
            AuthorId = authorId;
            BoxId = boxId;
        }

        public IdeaBoxLike(ApplicationUser user, IdeaBox box)
        {
            Author = user;
            Box = box;
        }
    }
}
