using Mvc.ApplicationCore.Entities.IdeaEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs
{
    public class CreateIdeaDto
    {
        public string Author { get; set; }
        public string Title {  get; set; }
        public string Description {  get; set; }
        public ICollection<string> Tags {  get; set; }
        public bool IsSecret {  get; set; }
        public IdeaStatuses Status { get; set; }

        public CreateIdeaDto()
        {
            Status = IdeaStatuses.FindMembers;
        }
    }
}
