using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs.Idea
{
    public class IdeaMemberDto
    {
        public string Guid { get; set; }
        public string Name {  get; set; }
        public string AvatarImageName { get; set; }
        public List<TagDto> Tags { get; set; }
        public int Number { get; set; }
    }
}
