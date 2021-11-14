using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Entities.IdeaEntity
{
    public class IdeaStatusImage : BaseImage
    {
        public IdeaStatus IdeaStatus { get; set; }

        public IdeaStatusImage(string name)
        {
            Name = name;
        }

        public IdeaStatusImage(string name, IdeaStatus status)
        {
            Name = name;
            IdeaStatus = status;
        }
    }
}
