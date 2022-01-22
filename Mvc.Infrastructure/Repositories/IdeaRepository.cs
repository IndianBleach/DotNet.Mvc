using AutoMapper;
using Microsoft.AspNetCore.Http;
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
using Microsoft.AspNetCore.Hosting;

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

            int countFill = 4 - userTags.Count;

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
                    .OrderBy(x => x.Topics.Count()).Take(4).ToList();


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Idea, IdeaRecommendationDto>();
                cfg.AddProfile<RecommendIdeaProfile>();
            });

            var mapper = new Mapper(config);

            var dtos = mapper.Map<List<Idea>, List<IdeaRecommendationDto>>(recommendIdeas.Take(4).ToList());

            return dtos;
        }

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
        
        public string? CreateIdea(CreateIdeaDto item)
        {
            try
            {
                if (item.Author != null && item.Title != null && item.Tags != null && item.Description != null)
                {
                    ApplicationUser author = _dbContext.Users.FirstOrDefault(x => x.UserName == item.Author);

                    List<Tag> ideaTags = BuildTagList(item.Tags);

                    var config = new MapperConfiguration(conf => conf.CreateMap<CreateIdeaDto, Idea>()
                        .ForMember("IsSecurity", opt => opt.MapFrom(x => x.IsSecret))
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
                            .FirstOrDefault(y => y.ImageName.Equals("idea_def_back.png")))));

                    var mapper = new Mapper(config);

                    var idea = mapper.Map<Idea>(item);

                    _dbContext.Ideas.Add(idea);

                    return idea.Guid.ToString();
                }
            }
            catch (Exception exp)
            {
                return null;
            }

            return null;
        }

        private List<Tag> BuildTagList(ICollection<string> tags)
        {
            List<Tag> tagList = new();
            if (tags != null)
            {
                foreach (var item in tags)
                {
                    Tag? getTag = _dbContext.Tags
                        .FirstOrDefault(x => x.Name.Equals(item));

                    if (getTag != null)
                        tagList.Add(getTag);
                }
            }

            return tagList;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

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

        private static string GeneratePublishDate(DateTime dateCreated)
        {
            var res = DateTime.Now - dateCreated;

            int years = 0;
            int months = 0;
            for (int i = 1; i <= res.Days; i++)
            {
                if (i > 30) months++;
                if (i > 364) years++;
            }

            if (years > 0) return $"{years} year(s) ago";
            else if (months > 0) return $"{months} month(s) ago";
            else if (res.Days < 1) return "today";
            else return $"{res.Days} day(s) ago";
        }

        public async Task<List<IdeaTopicDto>> GetIdeaTopicsAsync(string ideaGuid, int page)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IdeaTopic, IdeaTopicDto>()
                .ForMember("Guid", opt => opt.MapFrom(x => x.Guid))
                .ForMember("Title", opt => opt.MapFrom(x => x.Title))
                .ForMember("AuthorAvatarImageName", opt => opt.MapFrom(x => x.Author.Avatar.ImageName))
                .ForMember("AuthorGuid", opt => opt.MapFrom(x => x.Author.Id))
                .ForMember("Description", opt => opt.MapFrom(x => x.Description))
                .ForMember("CommentsCount", opt => opt.MapFrom(x => x.Comments.Count()))
                .ForMember("DateCreated", opt => opt.MapFrom(x => GeneratePublishDate(x.DateCreated)));                
            });

            var mapper = new Mapper(config);

            var topics = await _dbContext.IdeaTopics
                .Include(x => x.Idea)
                .Include(x => x.Author)
                .ThenInclude(x => x.Avatar)
                .Include(x => x.Comments)
                .Where(x => x.Idea.Guid.ToString() == ideaGuid)
                .Skip(10 * (page - 1))
                .Take(10)
                .ToListAsync();

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
            var idea = await _dbContext.Ideas
                .FirstOrDefaultAsync(x => x.Guid.ToString() == ideaGuid);

            var createTopic = new IdeaTopic(title, description, idea.Id, authorGuid);

            _dbContext.IdeaTopics.Add(createTopic);

            _dbContext.SaveChanges();

            return true;
        }

        public async Task<List<IdeaRoleDto>> GetIdeaRolesAsync(string ideaGuid)
        {
            var roles = await _dbContext.IdeaMemberRoles
                .Include(x => x.Idea)
                .Include(x => x.User)
                .ThenInclude(x => x.Avatar)
                .Where(x => x.Idea.Guid.ToString() == ideaGuid &&
                    x.Role != IdeaMemberRoles.Author)
                .ToListAsync();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IdeaMemberRole, IdeaRoleDto>()
                .ForMember("RoleGuid", opt => opt.MapFrom(x => x.Guid))
                .ForMember("UserAvatarImage", opt => opt.MapFrom(x => x.User.Avatar.ImageName))
                .ForMember("UserName", opt => opt.MapFrom(x => x.User.UserName))
                .ForMember("UserGuid", opt => opt.MapFrom(x => x.User.Id))
                .ForMember("IdeaGuid", opt => opt.MapFrom(x => x.Idea.Guid))
                .ForMember("IsModder", opt => opt.MapFrom(x => x.Role.Equals(IdeaMemberRoles.Modder)));                
            });

            var mapper = new Mapper(config);

            var res = mapper.Map<List<IdeaMemberRole>, List<IdeaRoleDto>>(roles);

            return res;
        }

        public async Task<bool> AddModder(string roleGuid)
        {
            var getRole = await _dbContext.IdeaMemberRoles
                .FirstOrDefaultAsync(x => x.Guid.ToString() == roleGuid);

            getRole.Role = IdeaMemberRoles.Modder;

            _dbContext.IdeaMemberRoles.Update(getRole);

            return true;
        }

        public async Task<bool> RemoveModder(string roleGuid)
        {
            var getRole = await _dbContext.IdeaMemberRoles
                .FirstOrDefaultAsync(x => x.Guid.ToString() == roleGuid);

            getRole.Role = IdeaMemberRoles.Default;

            _dbContext.IdeaMemberRoles.Update(getRole);

            return true;
        }

        public async Task<string> CickMember(string roleGuid)
        {
            var getRole = await _dbContext.IdeaMemberRoles
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Guid.ToString() == roleGuid);

            _dbContext.IdeaMemberRoles.Remove(getRole);

            return getRole.User.UserName;
        }

        public async Task<TopicDetailDto> GetTopicDetail(string topicGuid)
        {
            var res = await _dbContext.IdeaTopics
                .Include(x => x.Idea.Avatar)
                .Include(x => x.Author)
                .ThenInclude(x => x.Avatar)
                .Include(x => x.Comments)
                .ThenInclude(x => x.Author)
                .ThenInclude(x => x.Avatar)
                .FirstOrDefaultAsync(x => x.Guid.ToString() == topicGuid);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IdeaTopic, TopicDetailDto>()
                .ForMember("Guid", opt => opt.MapFrom(x => x.Guid))
                .ForMember("AuthorAvatarImage", opt => opt.MapFrom(x => x.Author.Avatar.ImageName))
                .ForMember("AuthorGuid", opt => opt.MapFrom(x => x.Author.Id))
                .ForMember("DatePublished", opt => opt.MapFrom(x => GeneratePublishDate(x.DateCreated)))
                .ForMember("IdeaAvatarImage", opt => opt.MapFrom(x => x.Idea.Avatar.ImageName))
                .ForMember("Title", opt => opt.MapFrom(x => x.Title))
                .ForMember("Description", opt => opt.MapFrom(x => x.Description))
                .ForMember("Comments", opt => opt.MapFrom(x => x.Comments.Select(e => 
                    new TopicCommentDto(e.Author.Id,
                    e.Author.Avatar.ImageName,
                    e.Author.UserName,
                    GeneratePublishDate(e.DateCreated),
                    e.Message)
                )));
            });

            var mapper = new Mapper(config);

            TopicDetailDto dto = mapper.Map<IdeaTopic, TopicDetailDto>(res);

            return dto;
        }

        public async Task<TopicCommentDto> CreateTopicComment(string topicGuid, string authorGuid, string message)
        {
            var getTopic = await _dbContext.IdeaTopics
                .FirstOrDefaultAsync(x => x.Guid.ToString() == topicGuid);

            var getAuthor = await _dbContext.Users
                .Include(x => x.Avatar)                
                .FirstOrDefaultAsync(x => x.Id.Equals(authorGuid));

            var createComment = new TopicComment(getTopic, authorGuid, message);

            await _dbContext.IdeaTopicComments
                .AddAsync(createComment);

            var dto = new TopicCommentDto()
            {
                AuthorAvatarImage = getAuthor.Avatar.ImageName,
                AuthorGuid = getAuthor.Id,
                AuthorName = getAuthor.UserName,
                Comment = message,
                DateCreated = GeneratePublishDate(createComment.DateCreated)
            };

            return dto;
        }

        public async Task<bool> CreateBoxAsync(string title, string description, bool isAuthored, string authorGuid, string ideaGuid)
        {
            var getIdea = await _dbContext.Ideas
                .FirstOrDefaultAsync(x => x.Guid.ToString() == ideaGuid);

            if (getIdea != null)
            {
                if (!string.IsNullOrWhiteSpace(title) &&
                    !string.IsNullOrWhiteSpace(description) &&
                    !string.IsNullOrWhiteSpace(authorGuid))
                {
                    var createBox = new IdeaBox(title, description, isAuthored, authorGuid, getIdea.Id);

                    await _dbContext.IdeaBoxes.AddAsync(createBox);

                    return true;
                }
            }

            return false;
        }

        public async Task<List<IdeaBoxDto>> GetIdeaBoxesAsync(string ideaGuid, string currentUserGuid, int page)
        {
            var boxes = await _dbContext.IdeaBoxes
                .Include(x => x.Idea)
                .Include(x => x.Author)
                .Include(x => x.Goals)
                .ThenInclude(x => x.Status)
                .Where(x => x.Idea.Guid.ToString() == ideaGuid)
                .Skip(10 * (page - 1))
                .Take(10)
                .ToListAsync();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IdeaBox, IdeaBoxDto>()
                .ForMember("Guid", opt => opt.MapFrom(x => x.Guid))
                .ForMember("Title", opt => opt.MapFrom(x => x.Name))
                .ForMember("AuthorGuid", opt => opt.MapFrom(x => x.Author.Id))
                .ForMember("AuthorAvatarImageName", opt => opt.MapFrom(x => x.Author.Avatar.ImageName))
                .ForMember("Description", opt => opt.MapFrom(x => x.Description))
                .ForMember("GoalsCount", opt => opt.MapFrom(x => x.Goals.Count))
                .ForMember("DateCreated", opt => opt.MapFrom(x => GeneratePublishDate(x.DateCreated)))
                .ForMember("IsAuthored", opt => opt.MapFrom(x => x.IsAuthored))
                .ForMember("CurrentUserBox", opt => opt.MapFrom(x => x.Author.Id == currentUserGuid))
                .ForMember("CountCompleteGoals", opt => opt.MapFrom(x => x.Goals
                    .Where(x => x.Status.Type.Equals(BoxGoalStatuses.Complete))
                    .Count()))
                .ForMember("CountFailedGoals", opt => opt.MapFrom(x => x.Goals
                    .Where(x => x.Status.Type.Equals(BoxGoalStatuses.Failed))
                    .Count()))
                .ForMember("CountWaitingGoals", opt => opt.MapFrom(x => x.Goals
                    .Where(x => x.Status.Type.Equals(BoxGoalStatuses.Waiting))
                    .Count()));
            });

            var mapper = new Mapper(config);

            List<IdeaBoxDto> dto = mapper.Map<List<IdeaBox>, List<IdeaBoxDto>>(boxes);

            return dto;
        }

        public async Task<BoxDetailDto> GetBoxDetailAsync(string boxGuid, string currentUserGuid)
        {
            var box = await _dbContext.IdeaBoxes
                .Include(x => x.Idea)
                .Include(x => x.Author)
                .ThenInclude(x => x.Avatar)
                .Include(x => x.Goals)
                .ThenInclude(x => x.Author)
                .ThenInclude(x => x.Avatar)
                .Include(x => x.Goals)
                .ThenInclude(x => x.Status)
                .FirstOrDefaultAsync(x => x.Guid.ToString() == boxGuid);

            var getRole = await _dbContext.IdeaMemberRoles
                .Include(x => x.User)
                .Include(x => x.Idea)
                .FirstOrDefaultAsync(x => x.Idea == box.Idea && x.UserId.Equals(currentUserGuid));

            var checkedRole = getRole != null ? getRole.Role : IdeaMemberRoles.Default; 

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IdeaBox, BoxDetailDto>()
                .ForMember("Guid", opt => opt.MapFrom(x => x.Guid))
                .ForMember("AuthorAvatarImage", opt => opt.MapFrom(x => x.Author.Avatar.ImageName))
                .ForMember("AuthorGuid", opt => opt.MapFrom(x => x.Author.Id))
                .ForMember("DatePublished", opt => opt.MapFrom(x => GeneratePublishDate(x.DateCreated)))
                .ForMember("Title", opt => opt.MapFrom(x => x.Name))
                .ForMember("Description", opt => opt.MapFrom(x => x.Description))
                .ForMember("IsAuthored", opt => opt.MapFrom(x => x.IsAuthored))
                .ForMember(x => x.Goals, opt => opt.MapFrom(x => x.Goals.Select(goal =>
                    new BoxGoalDetailDto() {
                        Guid = goal.Guid.ToString(),
                        AuthorGuid = goal.Author.Id,
                        AuthorAvatarImage = goal.Author.Avatar.ImageName,
                        AuthorName = goal.Author.UserName,
                        DatePublished = GeneratePublishDate(goal.DateCreated),
                        Content = goal.Description,
                        Status = goal.Status.Type,
                        CanEdit = checkedRole < IdeaMemberRoles.Default
                     }
                )));
            });

            var mapper = new Mapper(config);

            BoxDetailDto dto = mapper.Map<IdeaBox, BoxDetailDto>(box);

            return dto;
        }

        public async Task<List<IdeaMemberDto>> GetIdeaMembersAsync(string ideaGuid, int page)
        {
            var members = await _dbContext.IdeaMemberRoles
                .Include(x => x.Idea)
                .Include(x => x.User)
                .ThenInclude(x => x.Tags)
                .Include(x => x.User)
                .ThenInclude(x => x.Avatar)
                .Where(x => x.Idea.Guid.ToString() == ideaGuid)
                .Skip(10 * (page - 1))
                .Take(10)
                .ToListAsync();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IdeaMemberRole, IdeaMemberDto>()
                .ForMember("Guid", opt => opt.MapFrom(x => x.User.Id))
                .ForMember("Name", opt => opt.MapFrom(x => x.User.UserName))
                .ForMember("AvatarImageName", opt => opt.MapFrom(x => x.User.Avatar.ImageName))
                .ForMember("Tags", opt => opt.MapFrom(x => x.User.Tags.Select(e => new TagDto(e.Name))));
            });

            var mapper = new Mapper(config);

            List<IdeaMemberDto> dto = mapper.Map<List<IdeaMemberRole>, List<IdeaMemberDto>>(members);

            int memberNumber = 1;
            foreach (var item in dto)
            {
                item.Number = memberNumber;
                memberNumber++;
            }

            return dto;
        }

        public async Task<BoxGoalDetailDto> CreateBoxGoalAsync(string boxGuid, string authorGuid, string content)
        {
            BoxGoalStatus getStatus = await _dbContext.GoalStatuses
                .FirstOrDefaultAsync(x => x.Type.Equals(BoxGoalStatuses.Waiting));

            IdeaBox getBox = await _dbContext.IdeaBoxes
                .FirstOrDefaultAsync(x => x.Guid.ToString() == boxGuid);

            IdeaMemberRole getRole = await _dbContext.IdeaMemberRoles
                .Include(x => x.User)
                .ThenInclude(x => x.Avatar)
                .FirstOrDefaultAsync(x => x.User.Id.Equals(authorGuid));

            BoxGoal newGoal = new BoxGoal(authorGuid, content, getStatus.Id, getBox.Id);

            _dbContext.BoxGoals.Add(newGoal);

            BoxGoalDetailDto dto = new()
            {
                AuthorAvatarImage = getRole.User.Avatar.ImageName,
                AuthorName = getRole.User.UserName,
                Content = content,
                DatePublished = GeneratePublishDate(newGoal.DateCreated),
                Guid = newGoal.Guid.ToString(),
                Status = newGoal.Status.Type,
                CanEdit = getRole.Role < IdeaMemberRoles.Default
            };

            return dto;
        }

        public async Task<bool> RemoveGoalAsync(string goalGuid, string currentUserGuid)
        {
            var getGoal = await _dbContext.BoxGoals
                .FirstOrDefaultAsync(x => x.Guid.ToString() == goalGuid);

            if (getGoal != null)
            {
                _dbContext.BoxGoals.Remove(getGoal);
                return true;
            }
            else return false;
        }

        public async Task<BoxGoalDetailDto> UpdateGoalStatusAsync(string goalGuid, BoxGoalStatuses newStatus, string currentUserGuid)
        {
            var getGoal = await _dbContext.BoxGoals
                .Include(x => x.Status)
                .Include(x => x.Author)
                .ThenInclude(x => x.Avatar)
                .FirstOrDefaultAsync(x => x.Guid.ToString() == goalGuid);

            if (getGoal != null)
            {
                getGoal.Status = await _dbContext.GoalStatuses
                        .FirstOrDefaultAsync(x => x.Type.Equals(newStatus));

                _dbContext.BoxGoals.Update(getGoal);
            }

            BoxGoalDetailDto dto = new()
            {
                AuthorAvatarImage = getGoal.Author.Avatar.ImageName,
                AuthorGuid = getGoal.Author.Id,
                DatePublished = GeneratePublishDate(getGoal.DateCreated),
                AuthorName = getGoal.Author.UserName,
                CanEdit = true,
                Content = getGoal.Description,
                Guid = getGoal.Guid.ToString(),
                Status = getGoal.Status.Type
            };

            return dto;
        }

        public async Task<string> UpdateIdeaSettingsAsync(IFormFile avatar, IdeaStatuses status, string description, bool isSecurity, string ideaGuid, string currentUserGuid)
        {
            var getIdea = await _dbContext.Ideas
                .Include(x => x.Avatar)
                .Include(x => x.Members)
                .Include(x => x.Status)
                .Include(x => x.Topics)
                .FirstOrDefaultAsync(x => x.Guid.ToString() == ideaGuid);

            bool isUserAuthor = false;
            if (getIdea != null)
            {
                isUserAuthor = getIdea.Members.Any(x => x.UserId.Equals(currentUserGuid) &&
                    x.Role.Equals(IdeaMemberRoles.Author));

                if (avatar != null)
                {
                    using (FileStream str = new FileStream($"wwwroot/media/ideaAvatars/"+ avatar.FileName , FileMode.Create))
                    {
                        await avatar.CopyToAsync(str);
                        str.Close();
                    }                    

                    if (getIdea.Avatar.ImageName != "def_idea_avatar.png")
                    {
                        File.Delete($"wwwroot/media/ideaAvatars/{getIdea.Avatar.ImageName}");
                    }

                    getIdea.Avatar = new IdeaAvatarImage(avatar.FileName);
                }
                if (!string.IsNullOrEmpty(status.ToString()))
                {
                    getIdea.Status = await _dbContext.IdeaStatuses
                        .FirstOrDefaultAsync(x => x.Status.Equals(status));
                }
                if (!string.IsNullOrEmpty(description))
                {
                    getIdea.Topics
                        .FirstOrDefault(x => x.IsDefault == true)
                        .Description = description;
                }

                getIdea.IsSecurity = isSecurity;
            }          

            return getIdea.Guid.ToString();
        }

        public async Task<bool> RemoveIdeaAsync(string ideaGuid, string confirmPassword, string currentUserGuid)
        {
            var getRole = await _dbContext.IdeaMemberRoles
                .Include(x => x.Idea)  
                .ThenInclude(x => x.Topics)
                .ThenInclude(x => x.Comments)
                .Include(x => x.Idea)
                .ThenInclude(x => x.Boxes)
                .ThenInclude(x => x.Goals)
                .Include(x => x.Idea)
                .ThenInclude(x => x.Members)                
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId.Equals(currentUserGuid) && x.Idea.Guid.ToString() == ideaGuid);

            bool isAuthor = false;
            if (getRole != null)
                if (getRole.Role.Equals(IdeaMemberRoles.Author))
                    isAuthor = true;

            if (isAuthor == true)
            {
                _dbContext.Ideas.Remove(getRole.Idea);

                return true;
            }

            return false;
        }

        public async Task<List<JoinRequestDto>> GetIdeaJoinRequests(string ideaGuid, int page)
        {
            var getIdea = await _dbContext.Ideas
                .FirstOrDefaultAsync(x => x.Guid.ToString() == ideaGuid);

            var res = await _dbContext.IdeaInvites
                .Include(x => x.Author)
                .ThenInclude(x => x.Avatar)
                .Include(x => x.InvitedUser)
                .ThenInclude(x => x.Avatar)
                .Include(x => x.RelateIdea)
                .Where(x => x.RelateIdea == getIdea && x.InviteType.Equals(InviteTypes.JoinRequest))
                .Skip(10 * (page - 1))
                .Take(10)
                .ToListAsync();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IdeaInvitation, JoinRequestDto>()
                .ForMember("InviteGuid", opt => opt.MapFrom(x => x.Guid))
                .ForMember("FromUserGuid", opt => opt.MapFrom(x => x.Author.Id))
                .ForMember("FromUserAvatar", opt => opt.MapFrom(x => x.Author.Avatar.ImageName))
                .ForMember("FromUserName", opt => opt.MapFrom(x => x.Author.UserName))
                .ForMember("Description", opt => opt.MapFrom(x => x.Content));
            });

            var mapper = new Mapper(config);

            var resp = mapper.Map<List<IdeaInvitation>, List<JoinRequestDto>>(res);

            return resp;
        }

        public async Task<bool> DeclineJoinAsync(string joinGuid)
        {
            var getJoin = await _dbContext.IdeaInvites
                .FirstOrDefaultAsync(x => x.Guid.ToString() == joinGuid);

            if (getJoin != null)
            {
                _dbContext.IdeaInvites.Remove(getJoin);
                return true;
            }

            return false;
        }

        public async Task<bool> AcceptJoinAsync(string joinGuid)
        {
            var getJoin = await _dbContext.IdeaInvites
                .Include(x => x.RelateIdea)
                .ThenInclude(x => x.Members)
                .Include(x => x.Author)
                .FirstOrDefaultAsync(x => x.Guid.ToString() == joinGuid);

            if (getJoin != null)
            {
                var createRole = new IdeaMemberRole(
                    IdeaMemberRoles.Default,
                    getJoin.AuthorId,
                    getJoin.RelateIdeaId);

                _dbContext.IdeaMemberRoles.Add(createRole);

                _dbContext.IdeaInvites.Remove(getJoin);

                return true;
            }

            return false;            
        }

        public List<SideIdeaDto> GetSimilarIdeas(string ideaGuid)
        {
            Idea getIdea = _dbContext.Ideas
                .Include(x => x.Tags)
                .FirstOrDefault(x => x.Guid.ToString() == ideaGuid);

            List<Idea> similarIdeas = new List<Idea>();

            if (getIdea != null)
            {
                int countFill = 5 - getIdea.Tags.Count;

                if (getIdea.Tags.Count > 0)
                {
                    for (int i = 0; i < getIdea.Tags.Count; i++)
                    {
                        similarIdeas.AddRange(_dbContext.Ideas
                            .Include(x => x.Avatar)
                            .Include(x => x.Tags)
                            .Where(x => x.Tags
                            .Contains(getIdea.Tags.ElementAt(i))).Take(countFill + 1));
                    }
                }                
            }
            else
                similarIdeas = _dbContext.Ideas
                    .Include(x => x.Avatar)
                    .Take(5)
                    .ToList();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Idea, SideIdeaDto>()
                .ForMember("Title", opt => opt.MapFrom(x => x.Title))
                .ForMember("Guid", opt => opt.MapFrom(x => x.Guid))
                .ForMember("AvatarImageName", opt => opt.MapFrom(x => x.Avatar.ImageName));

            });

            var mapper = new Mapper(config);

            var dtos = mapper.Map<List<Idea>, List<SideIdeaDto>>(similarIdeas.Take(5).ToList());

            return dtos;
        }
    }
}
