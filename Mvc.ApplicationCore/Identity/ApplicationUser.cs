using Microsoft.AspNetCore.Identity;
using Mvc.ApplicationCore.Entities;
using Mvc.ApplicationCore.Entities.IdeaEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string? Description { get; set; }
        public uint CompletedIdeasCount { get; set; }
        public uint CreatedIdeasCount { get; set; }
        public DateTime AccountDateCreated { get; set; }
        public ICollection<UserContact> Contacts { get; set; }
        public ICollection<IdeaMemberRole> IdeaMemberRoles { get; set; }
        public ICollection<IdeaActivity> IdeaActivity { get; set; }
        public ICollection<IdeaBoxLike> IdeaBoxLikes { get; set; }
        public ICollection<IdeaInvitation> IdeaInvites { get; set; }
        public ICollection<IdeaStar> IdeaStars { get; set; }
        public ICollection<IdeaBox> IdeaBoxes { get; set; }
        public ICollection<IdeaTopic> IdeaTopics { get; set; }
        public ICollection<Tag> Skills { get; set; }
        public ICollection<Follower> Followers { get; set; }
        public ICollection<Follower> Following { get; set; }
        public ICollection<ChatMessage> ChatMessagesFromAuthor { get; set; }
        public ICollection<ChatMessage> ChatMessagesToAuthor { get; set; }
        public ICollection<CommentMessage> Comments { get; set; }

        public ApplicationUser(string firstName, string userName, string passwordHash)
        {
            FirstName = firstName;
            base.UserName = userName;
            base.PasswordHash = passwordHash;
            CompletedIdeasCount = 0;
            CreatedIdeasCount = 0;
            AccountDateCreated = DateTime.Now;
            Contacts = new List<UserContact>();
            IdeaMemberRoles = new List<IdeaMemberRole>();
            IdeaActivity = new List<IdeaActivity>();
            IdeaBoxLikes = new List<IdeaBoxLike>();
            IdeaInvites = new List<IdeaInvitation>();
            IdeaStars = new List<IdeaStar>();
            IdeaBoxes = new List<IdeaBox>();
            IdeaTopics = new List<IdeaTopic>();
            Skills = new List<Tag>();
            Followers = new List<Follower>();
            Following = new List<Follower>();
            ChatMessagesFromAuthor = new List<ChatMessage>();
            ChatMessagesToAuthor = new List<ChatMessage>();
            Comments = new List<CommentMessage>();
        }
    }
}
