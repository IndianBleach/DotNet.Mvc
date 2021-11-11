using Microsoft.AspNetCore.Identity;
using Mvc.ApplicationCore.Entities;
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
        public string Description { get; set; }
        public uint CompletedIdeasCount { get; set; }
        public uint CreatedIdeasCount { get; set; }
        public DateTime AccountDateCreated { get; set; }
        public ICollection<UserContact> Contacts { get; set; }
        public ICollection<IdeaMemberRole> IdeaMemberRoles { get; set; }
        public ICollection<IdeaActivity> IdeaActivity { get; set; }
        public ICollection<IdeaBoxLike> IdeaBoxLikes { get; set; }
        public ICollection<IdeaInvite> IdeaInvites { get; set; }
        public ICollection<IdeaStar> IdeaStars { get; set; }
        public ICollection<IdeaBox> IdeaBoxes { get; set; }
        public ICollection<IdeaTopic> IdeaTopics { get; set; }
        public ICollection<Tag> Skills { get; set; }
        public ICollection<Follower> Followers { get; set; }
        public ICollection<Follower> Following { get; set; }
        public ICollection<ChatMessage> ChatMessagesFromAuthor { get; set; }
        public ICollection<ChatMessage> ChatMessagesToAuthor { get; set; }
    }
}
