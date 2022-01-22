using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.News
{
    public class NewsDto
    {
        public string UserAvatarName { get; set; }
        public string UserGuid { get; set; }
        public string UserName {  get; set; }
        public string? IdeaGuid { get; set; }
        public string? IdeaName { get; set; }
        public DateTime Date { get; set; }
    }
}
