using Mvc.ApplicationCore.Entities.IdeaEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.Idea
{
    public class BoxGoalDto
    {
        public string Guid { get; set; }
        public string AuthorGuid { get; set; }
        public string AuthorAvatarImage { get; set; }
        public string AuthorName { get; set; }
        public string DateCreated { get; set; }
        public string Description {  get; set; }
        public BoxGoalStatuses Status { get; set; }

        public BoxGoalDto()
        {
        }

        public BoxGoalDto(string guid,
            string authorGuid,
            string avatar,
            string authorName,
            string dateCreated,
            string description,
            BoxGoalStatuses status)
        {
            Guid = guid;
            AuthorGuid = authorGuid;
            AuthorAvatarImage = avatar;
            AuthorName = authorName;
            DateCreated = dateCreated;
            Description = description;
            Status = status;
        }
    }

    public class BoxDetailDto
    {
        public string Guid { get; set; }
        public string AuthorAvatarImage { get; set; }
        public string AuthorGuid { get; set; }
        public string DatePublished { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsAuthored { get; set; }
        public List<BoxGoalDetailDto> Goals { get; set; }
    }
}
