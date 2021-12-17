using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.Idea
{

    public class IdeaAuthorDto
    { 
        public Guid Guid {  get; set; }
        public string Name {  get; set; }
        public string AvatarName { get; set; }
    }

    public class HomeIdeaDto
    {
        public string Guid {  get; set; }
        public string Title { get; set; }
        public string Description {  get; set; }
        public string AvatarImageName {  get; set; }
        public int TotalCommentsCount { get; set; }
        public int TotalBoxCount { get; set; }
        public ICollection<TagDto> Tags { get; set; }
    }
}
