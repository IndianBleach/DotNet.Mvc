using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Entities.IdeaEntity
{
    public class IdeaAvatarImage
    { 
        public int Id { get; set; }
        public string ImageName {  get; set; }
        public ICollection<Idea> Ideas { get; set; }

        public IdeaAvatarImage(string imageName)
        {
            ImageName = imageName;
            Ideas = new List<Idea>();
        }
    }

    public class Idea : BaseEntity
    {
        public Guid Guid { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public int StatusId { get; set; }
        public IdeaStatus Status { get; set; }
        public ICollection<IdeaTopic> Topics { get; set; }
        public ICollection<IdeaMemberRole> Members { get; set; }
        public ICollection<IdeaBox> Boxes { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<IdeaStar> Stars { get; set; }
        public int AvatarId { get; set; }
        public IdeaAvatarImage Avatar { get; set; }
        public bool IsSecurity { get; set; }

        public Idea()
        {
            IsSecurity = false;
            Guid = Guid.NewGuid();
            DateCreated = DateTime.Now;
            DateUpdated = DateTime.Now;
            Boxes = new List<IdeaBox>();
            Stars = new List<IdeaStar>();
        }

        public Idea(string title, int statusId)
        {
            IsSecurity = false;
            StatusId = statusId;
            Title = title;
            Topics = new List<IdeaTopic>();
            Members = new List<IdeaMemberRole>();
            Boxes = new List<IdeaBox>();
            Tags = new List<Tag>();
            Stars = new List<IdeaStar>();
            Guid = Guid.NewGuid();
            DateCreated = DateTime.Now;
            DateUpdated = DateTime.Now;
        }

        public Idea(
            string title,
            IdeaMemberRole initMember,
            IdeaStatus initStatus,
            IdeaTopic initTopic,
            bool isSecurity
            )
        {
            IsSecurity = isSecurity;
            Members = new List<IdeaMemberRole>() { initMember };
            Topics = new List<IdeaTopic>() { initTopic };
            Status = initStatus;
            Boxes = new List<IdeaBox>();
            Tags = new List<Tag>();
            Stars = new List<IdeaStar>();
            Guid = Guid.NewGuid();
            Title = title;
        }
    }
}
