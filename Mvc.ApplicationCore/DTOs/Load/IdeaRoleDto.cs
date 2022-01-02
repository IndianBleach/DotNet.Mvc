using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.Load
{
    public class IdeaRoleDto
    {
        public string UserAvatarImage { get; set; }
        public string UserName {  get; set; }
        public string UserGuid { get; set; }
        public string IdeaGuid { get; set; }
        public bool IsModder { get; set; } 
    }
}
