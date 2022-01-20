using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.Entities;
using Mvc.ApplicationCore.Identity;
using Mvc.ApplicationCore.Interfaces;
using Mvc.Infrastructure.Data;

namespace Mvc.Infrastructure.Services
{
    public class TagService : ITagService
    {
        private readonly ApplicationContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public TagService(ApplicationContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public ICollection<TagDto> GetUserTags(string userName)
        {
            var config = new MapperConfiguration(conf => conf.CreateMap<Tag, TagDto>()
            .ForMember("Name", opt => opt.MapFrom(x => x.Name)));

            var mapper = new Mapper(config);

            var user = _dbContext.Users
                .Include(x => x.Tags)
                .FirstOrDefault(x => x.UserName.Equals(userName));

            List<TagDto> dtos = new();
            if (user != null)
                dtos = mapper.Map<List<Tag>, List<TagDto>>(user.Tags.ToList());

            return dtos;
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
