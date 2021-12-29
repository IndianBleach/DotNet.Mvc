using Mvc.ApplicationCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Entities
{
    public class ChatMessage : BaseEntity
    {
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public string Text { get; set; }
        public DateTime DateCreated { get; set; }
        public Chat Chat { get; set; }

        public ChatMessage(string authorId, string text)
        {
            AuthorId = authorId;
            Text = text;
            DateCreated = DateTime.Now;
        }

        public ChatMessage(ApplicationUser author, string text, Chat chat)
        {
            AuthorId = author.Id;
            Author = author;
            Text = text;
            DateCreated = DateTime.Now;
            Chat = chat;
        }
    }

    public class ChatUser : BaseEntity
    { 
        public string UserId {  get; set; }
        public ApplicationUser User { get; set; }
        public string ChatId { get; set; }
        public Chat Chat {  get; set; }

        public ChatUser(ApplicationUser user, Chat chat)
        {
            UserId = user.Id;
            User = user;
            ChatId = chat.Id.ToString();
            Chat = chat;
        }

        public ChatUser(string userId, string chatId)
        {
            UserId = userId;
            ChatId = chatId;
        }
    }

    public class Chat : BaseEntity
    { 
        public Guid Guid { get; set; }
        public ICollection<ChatUser> Users {  get; set; }
        public ICollection<ChatMessage> Messages { get; set; }

        public Chat()
        {
            Guid = Guid.NewGuid();
            Messages = new List<ChatMessage>();
            Users = new List<ChatUser>();
        }
    }

}
