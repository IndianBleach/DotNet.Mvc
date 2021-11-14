using Mvc.ApplicationCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Entities
{
    public class UserContact : BaseEntity
    {
        public string Name {  get; set; }
        public string Url { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public UserContact(
            string name,
            string url,
            int userId)
        {
            Name = name;
            Url = url;
            UserId = userId;
        }

        public UserContact(
            string contactName,
            string url,
            ApplicationUser user)
        {
            Name = contactName;
            Url = url;
            User = user;
        }
    }
}
