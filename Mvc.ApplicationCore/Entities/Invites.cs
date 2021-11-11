using Mvc.ApplicationCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Entities
{
    public enum InviteTypes
    {
        JoinRequest,
        Invite
    }

    public class IdeaInvite
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public ApplicationUser Author { get; set; }
        public Idea ToIdea { get; set; }
        public InviteTypes InviteType { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
