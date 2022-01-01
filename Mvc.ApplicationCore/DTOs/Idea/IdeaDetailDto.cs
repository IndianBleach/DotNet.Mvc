using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.Idea
{

    public class IdeaModderDto
    { 
        public string AvatarImageName { get; set; }
        public string Guid { get; set; }

        public IdeaModderDto(string avatar, string guid)
        {
            AvatarImageName = avatar;
            Guid = guid;
        }
    }

    public class IdeaStatusDto
    { 
        public string Title { get; set; }
        public string Description {  get; set; }

        public IdeaStatusDto(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }

    public class IdeaDetailDto
    {
        public string AvatarImageName { get; set; }
        public string Title { get; set; }
        public IdeaStatusDto Status { get; set; }
        public List<IdeaModderDto> Modders { get; set; }
        public List<TagDto> Tags { get; set; }        
    }
}
