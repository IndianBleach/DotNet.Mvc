using Mvc.ApplicationCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Entities.IdeaEntity
{
    public enum BoxGoalStatuses
    { 
        Complete,
        Waiting,
        Failed
    }

    public class BoxGoalStatus : BaseEntity
    { 
        public string Name { get; set; }
        public BoxGoalStatuses Type { get; set; }
        public ICollection<BoxGoal> Goals { get; set; }

        public BoxGoalStatus(string name, BoxGoalStatuses type)
        {
            Name = name;
            Type = type;
            Goals = new List<BoxGoal>();
        }
    }

    public class BoxGoal : BaseEntity
    {
        public Guid Guid { get; set; }
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public int StatusId { get; set; }
        public BoxGoalStatus Status { get; set; }
        public int BoxId { get; set; }
        public IdeaBox Box {get;set;}

        public BoxGoal(string authorId, string description, int statusId, int boxId)
        {
            Guid = Guid.NewGuid();
            AuthorId = authorId;
            Description = description;
            StatusId = statusId;
            DateCreated = DateTime.Now;
            BoxId = boxId;
        }

        public BoxGoal(string authorId, string description, int statusId)
        {
            Guid = Guid.NewGuid();
            AuthorId = authorId;
            Description = description;
            StatusId = statusId;
            DateCreated = DateTime.Now;
        }
    }


    public class IdeaBox : BaseEntity
    {
        public Guid Guid {  get; set; }
        public string Name {  get; set; }
        public string Description {  get; set; }
        public DateTime DateCreated { get; set; }
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public int IdeaId { get; set; }
        public Idea Idea { get; set; }
        public ICollection<BoxGoal> Goals { get; set; }
        public bool IsAuthored { get; set; }

        public IdeaBox(string name, string description, bool isAuthored, string authorId, int ideaId)
        {
            Guid = Guid.NewGuid();
            Name = name;
            Description = description;
            AuthorId = authorId;
            IdeaId = ideaId;
            DateCreated = DateTime.Now;
            Goals = new List<BoxGoal>();
            IsAuthored = isAuthored;
        }
    }
}
