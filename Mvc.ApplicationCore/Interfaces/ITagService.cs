using Mvc.ApplicationCore.DTOs;
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
        ICollection<TagDto> GetAllTags();
        ICollection<Tag> CreateTagList(ICollection<string> tags);
        ICollection<Tag> GetPopularTags();
    }
}