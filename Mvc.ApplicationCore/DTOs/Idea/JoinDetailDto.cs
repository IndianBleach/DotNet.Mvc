using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.Idea
{
    public class JoinDetailDto
    {
        public string UserGuid { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public string UserAvatarImage { get; set; }        
    }
}
