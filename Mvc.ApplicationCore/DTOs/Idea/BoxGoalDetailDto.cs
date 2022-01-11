using Mvc.ApplicationCore.Entities.IdeaEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.Idea
{
    public class BoxGoalDetailDto
    {
        public bool CanEdit { get; set; }
        public string Guid { get; set; }
        public string AuthorGuid { get; set; }
        public string AuthorName { get; set; }
        public string AuthorAvatarImage { get; set; }
        public string DatePublished { get; set; }
        public BoxGoalStatuses Status { get; set; }
        public string Content { get; set; }
    }
}
