using Mvc.ApplicationCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Entities.IdeaEntity
{
    public enum IdeaMemberRoles
    {
        Author = 1,
        Modder = 2,
        Default = 3,
        Investor = 4
    }

    public class IdeaMemberRole : BaseEntity
    {
        public IdeaMemberRoles Role { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int IdeaId { get; set; }
        public Idea Idea { get; set; }


        public IdeaMemberRole(IdeaMemberRoles role, ApplicationUser user)
        {
            User = user;
            Role = role;
        }

        public IdeaMemberRole(IdeaMemberRoles role, string userId)
        {
            UserId = userId;
            Role = role;
        }

        public IdeaMemberRole(
            IdeaMemberRoles role,
            string userId,
            int ideaId)
        {
            Role = role;
            UserId = userId;
            IdeaId = ideaId;
        }

        public IdeaMemberRole(
            IdeaMemberRoles role,
            ApplicationUser user,
            Idea idea)
        {
            Role = role;
            User = user;
            Idea = idea;
        }
    }
}
