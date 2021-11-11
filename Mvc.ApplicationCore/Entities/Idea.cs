using Mvc.ApplicationCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Entities
{
    public class IdeaTopic
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public int AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public int IdeaId { get; set; }
        public Idea Idea { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
    }


    public class BaseImage
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class IdeaStatusImage : BaseImage
    {
        public int IdeaStatusId { get; set; }
        public IdeaStatus IdeaStatus { get; set; }
    }

    public enum EnumIdeaStatus
    {
        Complete,
        FindMembers,
        InDevelopment
    }

    public class IdeaStatus
    {
        public int Id { get; set; }
        public EnumIdeaStatus Status { get; set; }
        public IdeaStatusImage Image { get; set; }
        public ICollection<Idea> Ideas { get; set; }
    }


    public enum IdeaMemberRoles
    {
        Author = 1,
        Modder = 2,
        Default = 3,
        Investor = 4
    }

    public class IdeaMemberRole
    {
        public int Id { get; set; }
        public IdeaMemberRoles Role { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int IdeaId { get; set; }
        public Idea Idea { get; set; }
    }


    public class IdeaBoxLike
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public int BoxId { get; set; }
        public IdeaBox Box { get; set; }
    }

    public class IdeaBox
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public ApplicationUser Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<IdeaBoxLike> Likes { get; set; }
    }


    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Idea> Ideas { get; set; }
    }


    public enum IdeaActivityTypes
    {
        RelateMembers,
        RelateTopics,
        RelateBoxes
    }

    public class IdeaActivity
    {
        public int Id { get; set; }
        public int RelateUserId { get; set; }
        public ApplicationUser RelateUser { get; set; }
        public string Description { get; set; }
        public string HighlightName { get; set; }
        public IdeaActivityTypes Type { get; set; }
    }


    public class IdeaStar
    {
        public int Id {  get; set; }
        public ApplicationUser Author { get; set; }
        public Idea Idea { get; set; }
    }


    public class Idea
    {
        public int Id {  get; set; }
        public Guid Guid { get; set; }
        public string Title { get; set; }      
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated {  get; set; }        
        public IdeaStatus Status { get; set; }
        public ICollection<IdeaTopic> Topics { get; set; }
        public ICollection<IdeaMemberRole> Members { get; set; }
        public ICollection<IdeaBox> Boxes { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<IdeaActivity> Activity { get; set; } 
        public ICollection<IdeaStar> Stars { get; set; }
    }
}
