using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.Entities;
using Mvc.ApplicationCore.Interfaces;
using Mvc.Infrastructure.Data;

namespace Mvc.Infrastructure.Services
{
    public class TagService : ITagService
    {
        private readonly ApplicationContext _dbContext;
        //private IRepository<Tag> _tagRepository;

        public TagService(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
            //_tagRepository = tagRepository;
        }

        public ICollection<Tag> CreateTagList(ICollection<string> choicedTags)
        {
            List<Tag> tags = new List<Tag>();
            foreach (string tagName in choicedTags)
                tags.Add(_dbContext.Tags.FirstOrDefault(x => x.Name == tagName));

            return tags;
        }
        
        public ICollection<TagDto> GetAllTags()
        {
            var config = new MapperConfiguration(conf => conf.CreateMap<Tag, TagDto>());

            var mapper = new Mapper(config);

            List<TagDto> tags = mapper.Map<List<TagDto>>(_dbContext.Tags.ToList());

            return tags;
        }

        #region not used
        public ICollection<Tag> GetPopularTags()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
