using Microsoft.AspNetCore.Identity;
using Mvc.ApplicationCore.Entities;
using Mvc.ApplicationCore.Entities.IdeaEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Identity
{
    public class UserAvatarImage
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }

        public UserAvatarImage(string imageName)
        {
            ImageName = imageName;
            Users = new List<ApplicationUser>();
        }
    }

    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? Description { get; set; }
        public uint CompletedIdeasCount { get; set; }
        public uint CreatedIdeasCount { get; set; }
        public DateTime AccountDateCreated { get; set; }
        public ICollection<UserContact> Contacts { get; set; }
        public ICollection<IdeaMemberRole> IdeaMemberRoles { get; set; }
        public ICollection<IdeaInvitation> IdeaInvites { get; set; }
        public ICollection<IdeaStar> IdeaStars { get; set; }
        public ICollection<IdeaBox> IdeaBoxes { get; set; }
        public ICollection<IdeaTopic> IdeaTopics { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<Follower> Followers { get; set; }
        public ICollection<Follower> Following { get; set; }
        //public ICollection<Chat> ChatMessagesFromAuthor { get; set; }
        //public ICollection<Chat> ChatMessagesToAuthor { get; set; }
        public ICollection<CommentMessage> Comments { get; set; }
        public int AvatarId { get; set; }
        public UserAvatarImage Avatar { get; set; }
        public ICollection<ChatUser> Chats { get; set; }

        public ApplicationUser()
        {
            CompletedIdeasCount = 0;
            CreatedIdeasCount = 0;
            AccountDateCreated = DateTime.Now;
            Contacts = new List<UserContact>();
            IdeaMemberRoles = new List<IdeaMemberRole>();
            IdeaInvites = new List<IdeaInvitation>();
            IdeaStars = new List<IdeaStar>();
            IdeaBoxes = new List<IdeaBox>();
            IdeaTopics = new List<IdeaTopic>();
            Tags = new List<Tag>();
            Followers = new List<Follower>();
            Following = new List<Follower>();
            //ChatMessagesFromAuthor = new List<Chat>();
            //ChatMessagesToAuthor = new List<Chat>();
            Comments = new List<CommentMessage>();
            Chats = new List<ChatUser>();
        }

        public ApplicationUser(
            string firstName,
            string userName,
            string passwordHash)
        {
            FirstName = firstName;
            base.UserName = userName;
            base.PasswordHash = passwordHash;
            CompletedIdeasCount = 0;
            CreatedIdeasCount = 0;
            AccountDateCreated = DateTime.Now;
            Contacts = new List<UserContact>();
            IdeaMemberRoles = new List<IdeaMemberRole>();
            IdeaInvites = new List<IdeaInvitation>();
            IdeaStars = new List<IdeaStar>();
            IdeaBoxes = new List<IdeaBox>();
            IdeaTopics = new List<IdeaTopic>();
            Tags = new List<Tag>();
            Followers = new List<Follower>();
            Following = new List<Follower>();
            //ChatMessagesFromAuthor = new List<Chat>();
            //ChatMessagesToAuthor = new List<Chat>();
            Comments = new List<CommentMessage>();
            Chats = new List<ChatUser>();
        }
    }
}
