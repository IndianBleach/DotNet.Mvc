using Mvc.ApplicationCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Entities
{
    public class Follower
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public int FollowingId { get; set; }
        public ApplicationUser Following { get; set; }

        public Follower(int authorId, int followingId)
        {
            AuthorId = authorId;
            FollowingId = followingId;
        }

        public Follower(ApplicationUser author, ApplicationUser following)
        { 
            Author = author;
            Following = following;
        }
    }
}
