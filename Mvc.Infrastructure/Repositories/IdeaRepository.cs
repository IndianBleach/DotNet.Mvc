using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.DTOs.Idea;
using Mvc.ApplicationCore.DTOs.Load;
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

        
        
        
        
        //fix nontauth       
        public IEnumerable<IdeaRecommendationDto> GetRecommendIdeas(string? forUsername)
        {
            List<Tag> userTags = new List<Tag>();

            var res = _dbContext.Users
                .Include(x => x.Tags)
                .FirstOrDefault(x => x.UserName.Equals(forUsername));

            try
            {
                if (res != null)
                {
                    userTags.AddRange(res.Tags.ToList());
                }
            }
            catch (Exception exp)
            { 
                //
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

        //
        public IdeaMemberRoleDto GetIdeaMemberRole(string ideaGuid, string userGuid)
        {
            var res = _dbContext.IdeaMemberRoles
                .Include(x => x.Idea)
                .Include(x => x.User)
                .FirstOrDefault(x => x.Idea.Guid.ToString() == ideaGuid &&
                    x.User.Id.Equals(userGuid));

            if (res != null)
            {
                return res.Role switch
                {
                    IdeaMemberRoles.Author => IdeaMemberRoleDto.Author,
                    IdeaMemberRoles.Investor => IdeaMemberRoleDto.Investor,
                    IdeaMemberRoles.Modder => IdeaMemberRoleDto.Modder,
                    IdeaMemberRoles.Default => IdeaMemberRoleDto.Member,
                    _ => IdeaMemberRoleDto.Viewer,
                };
            }
            else return IdeaMemberRoleDto.Viewer;
        }

        public List<IdeaTopicDto> GetIdeaTopics(string ideaGuid)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IdeaTopic, IdeaTopicDto>()
                .ForMember("Guid", opt => opt.MapFrom(x => x.Guid))
                .ForMember("Title", opt => opt.MapFrom(x => x.Title))
                .ForMember("AuthorAvatarImageName", opt => opt.MapFrom(x => x.Author.Avatar.ImageName))
                .ForMember("AuthorGuid", opt => opt.MapFrom(x => x.Author.Id))
                .ForMember("Description", opt => opt.MapFrom(x => x.Description))
                .ForMember("CommentsCount", opt => opt.MapFrom(x => x.Comments.Count()));                
            });

            var mapper = new Mapper(config);

            var topics = _dbContext.IdeaTopics
                .Include(x => x.Idea)
                .Include(x => x.Author)
                .ThenInclude(x => x.Avatar)
                .Include(x => x.Comments)
                .Where(x => x.Idea.Guid.Equals(ideaGuid))
                .ToList();

            List<IdeaTopicDto> res = mapper.Map<List<IdeaTopic>, List<IdeaTopicDto>>(topics);

            return res;
        }

        public IdeaDetailDto GetIdeaDetail(string ideaGuid)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Idea, IdeaDetailDto>()
                .ForMember("Guid", opt => opt.MapFrom(x => x.Guid.ToString()))
                .ForMember("AvatarImageName", opt => opt.MapFrom(x => x.Avatar.ImageName))
                .ForMember("Title", opt => opt.MapFrom(x => x.Title))
                .ForMember("Status", opt => opt.MapFrom(x =>
                    new IdeaStatusDto(x.Status.Status.ToString(), x.Status.Description)))
                .ForMember("Modders", opt => opt.MapFrom(x => x.Members.Where(x => x.Role.Equals(IdeaMemberRoles.Modder) || 
                    x.Role.Equals(IdeaMemberRoles.Author))
                    .Select(x => new IdeaModderDto(x.User.Avatar.ImageName, x.User.Id))))
                .ForMember("Tags", opt => opt.MapFrom(x => x.Tags.Select(tag => new TagDto(tag.Name))));                
            });

            var mapper = new Mapper(config);

            Idea getIdea = _dbContext.Ideas
                .Include(x => x.Avatar)
                .Include(x => x.Status)
                .Include(x => x.Tags)
                .Include(x => x.Members)
                .ThenInclude(x => x.User)
                .ThenInclude(x => x.Avatar)
                .FirstOrDefault(x => x.Guid.ToString() == ideaGuid);

            IdeaDetailDto dto = mapper.Map<Idea, IdeaDetailDto>(getIdea);

            return dto;

        }

        public async Task<bool> CreateTopic(string title, string description, string authorGuid, string ideaGuid)
        {
            var createTopic = new IdeaTopic(title, description, authorGuid);

            var idea = await _dbContext.Ideas
                .Include(x => x.Topics)
                .FirstOrDefaultAsync(x => x.Guid.ToString() == ideaGuid);

            idea.Topics.Add(createTopic);

            return true;
        }

        public async Task<List<IdeaRoleDto>> GetIdeaRolesAsync(string ideaGuid)
        {
            var roles = _dbContext.IdeaMemberRoles
                .Include(x => x.Idea)
                .Include(x => x.User)
                .ThenInclude(x => x.Avatar)
                .Where(x => x.Idea.Guid.ToString() == ideaGuid &&
                    x.Role != IdeaMemberRoles.Author);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IdeaMemberRole, IdeaRoleDto>()
                .ForMember("UserAvatarImage", opt => opt.MapFrom(x => x.User.Avatar.ImageName))
                .ForMember("UserName", opt => opt.MapFrom(x => x.User.UserName))
                .ForMember("UserGuid", opt => opt.MapFrom(x => x.User.Id))
                .ForMember("IdeaGuid", opt => opt.MapFrom(x => x.Idea.Guid))
                .ForMember("IsModder", opt => opt.MapFrom(x => x.Role.Equals(IdeaMemberRoles.Modder)));                
            });

            var mapper = new Mapper(config);

            var res = mapper.Map<List<IdeaMemberRole>, List<IdeaRoleDto>>(roles.ToList());

            return res;
        }
        #endregion
    }
}
