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
        public string FromUserId { get; set; }
        public ApplicationUser FromUser { get; set; }
        public string ToUserId {  get; set; }
        public ApplicationUser ToUser { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }

        public ChatMessage(
            string fromUserId,
            string toUserId,
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
