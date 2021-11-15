using Mvc.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs
{
    public class CreateIdeaDto
    {
        public string Title { get; set; }
        public string Description {  get; set; }
        public ICollection<string> Tags { get; set; }
        public bool IsSecurity { get; set; }

        public CreateIdeaDto(
            string title,
            string description,
            ICollection<string> tags)
        {
            Title = title;
            Description = description;
            Tags = tags;
        }
    }
}
