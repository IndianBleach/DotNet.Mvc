using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs
{
    public class RecommendIdeaDto
    {
        public string AuthorName { get; private set; }
        public string IdeaName { get; private set; }
        public Guid IdeaGuid { get; private set; }

        public RecommendIdeaDto(string authorName, string ideaName, Guid ideaGuid)
        {
            AuthorName = authorName;
            IdeaName = ideaName;
            IdeaGuid = ideaGuid;
        }        
    }
}
