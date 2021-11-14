using Mvc.ApplicationCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Entities.IdeaEntity
{
    public enum InviteTypes
    {
        JoinRequest,
        Invite
    }

    public class IdeaInvitation
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public int RelateIdeaId { get; set; }
        public Idea RelateIdea { get; set; }
        public InviteTypes InviteType { get; set; }
        public DateTime DateCreated { get; set; }

        public IdeaInvitation(
            string content,
            int authorId,
            int relateIdeaId,
            InviteTypes inviteType)
        {
            Content = content;
            AuthorId = authorId;
            RelateIdeaId = relateIdeaId;
            InviteType = inviteType;
            DateCreated = DateTime.Now;
        }

        public IdeaInvitation(
            string descriptionContent,
            ApplicationUser author,
            Idea relateIdea,
            InviteTypes inviteType)
        {
            Content = descriptionContent;
            Author = author;
            RelateIdea = relateIdea;
            InviteType = inviteType;
            DateCreated = DateTime.Now;
        }
    }
}
