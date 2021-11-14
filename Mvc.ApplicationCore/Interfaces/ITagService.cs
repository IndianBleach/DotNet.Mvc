using Mvc.ApplicationCore.Entities;
using Mvc.ApplicationCore.Entities.IdeaEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Interfaces
{
    public interface ITagService
    {
        IEnumerable<Idea> GetIdeasByTag(string tagName);

        IEnumerable<Tag> GetPopular();

    }
}
