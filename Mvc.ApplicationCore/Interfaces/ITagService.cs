using Mvc.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Interfaces
{
    public interface ITagService
    {
        ICollection<Tag> GetPopularTags();
    }
}
