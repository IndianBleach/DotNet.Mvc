using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.User
{
    public class UserEditGeneralSettingsDto
    {
        public string NewUsername { get; set; }
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
        public string NewDescription { get; set; }
    }

    public class UserEditTagSettingsDto
    { 
        public ICollection<string> NewTags { get; set; }
    }
}
