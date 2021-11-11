using Mvc.ApplicationCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Entities
{
    public class BaseDiscussMessage
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public ApplicationUser Author { get; set; }
        public DateTime DateCreated { get; set; }
        public BaseDiscuss Discuss { get; set; }
    }

    public class BaseDiscuss
    {
        public int Id { get; set; }
        public ICollection<BaseDiscussMessage> Messages { get; set; }
    }

    public class BoxDiscuss : BaseDiscuss
    {
        public IdeaBox Box { get; set; }
    }

    public class TopicDiscuss : BaseDiscuss
    {
        public IdeaTopic Topic { get; set; }
    }
}
