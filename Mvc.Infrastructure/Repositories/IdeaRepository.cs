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

                         
        public IEnumerable<IdeaRecommendationDto> GetRecommendIdeas(string? forUsername)
        {
            List<Tag> userTags = new List<Tag>();
            if (forUsername != null)
            {
                userTags.AddRange(_userManager.FindByNameAsync(forUsername).Result.Tags.ToList());
            }

            List<Idea> recommendIdeas = new List<Idea>();

            int countFill = 5 - userTags.Count;

            if (userTags.Count > 0)
            {
                for (int i = 0; i < userTags.Count; i++)
                {
                    recommendIdeas.AddRange(_dbContext.Ideas
                        .Include(x => x.Members)
                            .ThenInclude(x => x.User)
                            .ThenInclude(x => x.Avatar)
                        .Where(x => x.Tags
                        .Contains(userTags.ElementAt(i))).Take(countFill + 1));
                }
            }
            else
                recommendIdeas = _dbContext.Ideas.Where(x => x.Status.Status.Equals(IdeaStatuses.FindMembers))
                    .Include(x => x.Members)
                        .ThenInclude(x => x.User)
                        .ThenInclude(x => x.Avatar)
                    .OrderBy(x => x.Topics.Count()).Take(5).ToList();


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Idea, IdeaRecommendationDto>();
                cfg.AddProfile<RecommendIdeaProfile>();
            });

            var mapper = new Mapper(config);

            var dtos = mapper.Map<List<Idea>, List<IdeaRecommendationDto>>(recommendIdeas.Take(5).ToList());

            return dtos;
        }


        #region ready
        public IEnumerable<SideIdeaDto> GetSideIdeasByStatusFilter(IdeaStatuses filterStatus)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Idea, SideIdeaDto>();
                cfg.AddProfile<SideIdeaProfile>();
            });

            var mapper = new Mapper(config);

            var ideas = _dbContext.Ideas
                .Include(x => x.Avatar)
                .Where(x => x.Status.Status.Equals(filterStatus))
                .Take(5)
                .ToList();

            return mapper.Map<List<Idea>, List<SideIdeaDto>>(ideas);
        }

        public List<HomeIdeaDto> GetIdeas(int page)
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
                .Skip(10 * (page - 1))
                .Take(10)
                .ToList();

            var dtoIdeas = mapper.Map<List<Idea>, List<HomeIdeaDto>>(ideas);

            return dtoIdeas;
        }

        public List<HomeIdeaDto> GetIdeas(string query, int page)
        {
            bool isTagQuery = false;

            List<string> tagList = _dbContext.Tags.Select(x => x.Name).ToList();

            if (tagList.Contains(query)) isTagQuery = true;

            List<Idea> ideas = new List<Idea>();
            if (isTagQuery)
            {
                ideas = _dbContext.Ideas
                .Include(x => x.Avatar)
                .Include(x => x.Topics)
                    .ThenInclude(x => x.Comments)
                .Include(x => x.Boxes)
                .Include(x => x.Tags)
                .Where(x => x.Tags
                    .Contains(_dbContext.Tags
                    .FirstOrDefault(x => x.Name.Equals(query))))
                .Skip(10 * (page - 1))
                .Take(10)
                .ToList();
            }
            else
            {
                ideas = _dbContext.Ideas
                .Include(x => x.Avatar)
                .Include(x => x.Topics)
                    .ThenInclude(x => x.Comments)
                .Include(x => x.Boxes)
                .Include(x => x.Tags)
                .Where(x => x.Title.StartsWith(query))
                .Skip(10 * (page - 1))
                .Take(10)
                .ToList();
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Idea, HomeIdeaDto>();
                cfg.AddProfile<HomeIdeaProfile>();
            });

            var mapper = new Mapper(config);

            return mapper.Map<List<Idea>, List<HomeIdeaDto>>(ideas);
        }

        public int GetCount()
        {
            return _dbContext.Ideas.Count();
        }

        public int GetCount(string query)
        {
            bool isTagQuery = false;

            List<string> tagList = _dbContext.Tags.Select(x => x.Name).ToList();

            if (tagList.Contains(query)) isTagQuery = true;

            if (isTagQuery) return _dbContext.Ideas
                    .Include(x => x.Tags)
                    .Where(x => x.Tags.Contains(_dbContext.Tags
                        .FirstOrDefault(x => x.Name
                    .Equals(query))))
                    .Count();
            else
            return _dbContext.Ideas
                    .Include(x => x.Tags)
                    .Where(x => x.Title.StartsWith(query))                
                    .Count();
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

        private List<Tag> BuildTagList(ICollection<string> tags)
        {
            List<Tag> tagList = new List<Tag>();
            foreach (var item in tags)
                tagList.Add(_dbContext.Tags
                    .FirstOrDefault(x => x.Name.Equals(item)));

            return tagList;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
        #endregion
    }
}
