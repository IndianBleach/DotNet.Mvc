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
        public int FromUserId { get; set; }
        public ApplicationUser FromUser { get; set; }
        public int ToUserId {  get; set; }
        public ApplicationUser ToUser { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }

        public ChatMessage(
            int fromUserId,
            int toUserId,
            string message)
        {   
            FromUserId = fromUserId;
            ToUserId = toUserId;
            Message = message;
            DateCreated = DateTime.Now;
        }

        public ChatMessage(
            ApplicationUser fromUser,
            ApplicationUser toUser,
            string message)
        {
            FromUser = fromUser;
            ToUser = toUser;
            Message = message;
            DateCreated = DateTime.Now;
        }
    }
}
