using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mvc.ApplicationCore.Entities;
using Mvc.ApplicationCore.Interfaces;
using Mvc.Infrastructure.Data;

namespace Mvc.Infrastructure.Services
{
    public class TagService : ITagService
    {
        private readonly ApplicationContext _dbContext;

        public TagService(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ICollection<Tag> CreateTagList(ICollection<string> choicedTags)
        {
            List<Tag> tags = new List<Tag>();
            foreach (string tagName in choicedTags)
                tags.Add(_dbContext.Tags.FirstOrDefault(x => x.Name == tagName));

            return tags;
        }

        public ICollection<Tag> GetPopularTags()
        {
            throw new NotImplementedException();
        }
    }
}
