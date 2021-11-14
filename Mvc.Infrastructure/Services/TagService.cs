using Mvc.ApplicationCore.Entities;
using Mvc.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mvc.Infrastructure.Data;
using Mvc.ApplicationCore.Entities.IdeaEntity;

namespace Mvc.Infrastructure.Services
{
    public class TagService : ITagService
    {
        private readonly ApplicationContext _dbContext;

        public TagService(ApplicationContext ctx)
        {
            _dbContext = ctx;
        }

        public IEnumerable<Idea> GetIdeasByTag(string tagName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tag> GetPopular()
        {
            return _dbContext.Tags
                .OrderBy(x => x.Ideas.Count())
                .Take(5);
        }
    }
}
