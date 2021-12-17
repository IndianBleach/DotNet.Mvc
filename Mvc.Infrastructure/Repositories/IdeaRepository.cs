using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.DTOs.Idea;
using Mvc.ApplicationCore.Entities;
using Mvc.ApplicationCore.Entities.IdeaEntity;
using Mvc.ApplicationCore.Identity;
using Mvc.ApplicationCore.Interfaces;
using Mvc.Infrastructure.Data;
using Mvc.Infrastructure.MapperProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.Infrastructure.Repositories
{
    public class IdeaRepository : IIdeaRepository
    {
        private ApplicationContext _dbContext;
        private UserManager<ApplicationUser> _userManager;

        public IdeaRepository(ApplicationContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        private List<Tag> BuildTagList(ICollection<string> tags)
        {
            List<Tag> tagList = new List<Tag>();
            foreach (var item in tags)
                tagList.Add(_dbContext.Tags
                    .FirstOrDefault(x => x.Name.Equals(item)));

            return tagList;
        }

        public Guid CreateIdea(CreateIdeaDto item)
        {
            ApplicationUser author = _dbContext.Users.FirstOrDefault(x => x.UserName.Equals(item.Author));

            List<Tag> ideaTags = BuildTagList(item.Tags);

            var config = new MapperConfiguration(conf => conf.CreateMap<CreateIdeaDto, Idea>()
                .ForMember("Title", opt => opt.MapFrom(x => x.Title))
                .ForMember("Tags", opt => opt.MapFrom(x => ideaTags))
                .ForMember("Topics", opt => opt.MapFrom(x => new List<IdeaTopic>()
                { 
                    new("About this idea", x.Description, author, true)
                }))
                .ForMember("Members", opt => opt.MapFrom(x => new List<IdeaMemberRole>()
                { 
                    new(IdeaMemberRoles.Author, author)
                }))
                .ForMember("Status", opt => opt.MapFrom(x => _dbContext.IdeaStatuses
                    .FirstOrDefault(y => y.Status.Equals(x.Status)))
                )
                .ForMember("Avatar", opt => opt.MapFrom(x => _dbContext.IdeaAvatars
                    .FirstOrDefault(y => y.ImageName.Equals("DEFAULT_IDEA_AVATAR.jpg")))));

            var mapper = new Mapper(config);

            var idea = mapper.Map<Idea>(item);

            _dbContext.Ideas.Add(idea);            

            return idea.Guid;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public IEnumerable<HomeIdeaDto> GetIdeasPerPage(int page)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Idea, HomeIdeaDto>();
                cfg.AddProfile<HomeIdeaProfile>();
            });

            var mapper = new Mapper(config);

            List<Idea> ideas = _dbContext.Ideas
                .Include(x => x.Avatar)
                .Include(x => x.Topics)
                    .ThenInclude(x => x.Comments)
                .Include(x => x.Boxes)
                .Include(x => x.Tags)
                .ToList();

            var dtoIdeas = mapper.Map<List<Idea>, List<HomeIdeaDto>>(ideas);

            return dtoIdeas;
        }
    }
}
