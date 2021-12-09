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

        public ICollection<Tag> GetPopularTags()
        {
            return _dbContext.Tags
                .OrderBy(x => x.Ideas.Count())
                .Take(5)
                .ToList();
        }
    }
}
