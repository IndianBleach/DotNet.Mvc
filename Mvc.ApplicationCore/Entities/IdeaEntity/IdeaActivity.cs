using Mvc.ApplicationCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Entities.IdeaEntity
{
    public enum IdeaActivityTypes
    {
        RelateMembers,
        RelateTopics,
        RelateBoxes,
        CreateIdea
    }

    public class IdeaActivity : BaseEntity
    {
        public int RelateUserId { get; set; }
        public ApplicationUser RelateUser { get; set; }
        public string Description { get; set; }
        public string HighlightName { get; set; }
        public IdeaActivityTypes Type { get; set; }

        public IdeaActivity(
            string description,
            string highlightName,
            IdeaActivityTypes type)
        {
            Description = description;
            HighlightName = highlightName;
            Type = type;
        }

        public IdeaActivity(
            ApplicationUser user,
            string description,
            string highlightName,
            IdeaActivityTypes type)
        {
            RelateUser = user;
            Description = description;
            HighlightName = highlightName;
            Type = type;
        }
    }
}
