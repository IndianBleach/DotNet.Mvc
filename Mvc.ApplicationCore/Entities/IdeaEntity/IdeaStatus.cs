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
        public int ImageId { get; set; }
        public IdeaStatusImage Image { get; set; }
        public ICollection<Idea> Ideas { get; set; }

        public IdeaStatus(
            int imageId,
            IdeaStatuses status,
            string description)
        {
            Description = description;
            ImageId = imageId;
            Status = status;
            Ideas = new List<Idea>();
        }

        public IdeaStatus(
            IdeaStatuses status,
            IdeaStatusImage img,
            string statusDescription)
        {
            Description = statusDescription;
            Status = status;
            Image = img;
            Ideas = new List<Idea>();
        }
    }
}
