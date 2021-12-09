using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Entities.IdeaEntity
{
    public enum IdeaStatuses
    {
        Complete,
        FindMembers,
        InDevelopment
    }

    public class IdeaStatus : BaseEntity
    {
        public string Description { get; set; }
        public IdeaStatuses Status { get; set; }
        public ICollection<Idea> Ideas { get; set; }

        public IdeaStatus(
            IdeaStatuses status,
            string description)
        {
            Description = description;
            Status = status;
            Ideas = new List<Idea>();
        }
    }
}
