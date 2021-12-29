using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.Chat
{
    public class MessageDto
    {
        public string AuthorGuid { get; set; }
        public string Message {  get; set; }
        public DateTime DateCreated { get; set; }
    }
}
