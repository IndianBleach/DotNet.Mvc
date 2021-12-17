using Mvc.ApplicationCore.Entities.IdeaEntity;
using Mvc.ApplicationCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Entities
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Idea> Ideas { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }

        public Tag(string name)
        {
            Name = name;
            Ideas = new List<Idea>();
        }
    }
}
