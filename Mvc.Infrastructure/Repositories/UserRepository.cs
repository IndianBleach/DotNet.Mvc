using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.DTOs.Chat;
using Mvc.ApplicationCore.DTOs.Idea;
using Mvc.ApplicationCore.DTOs.JsonResult;
using Mvc.ApplicationCore.DTOs.User;
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
    public class UserRepository : IUserRepository
    {
        private ApplicationContext _dbContext;
        private UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private ITagService _tagService;

        public UserRepository(ITagService tagService, ApplicationContext dbContext, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _dbContext = dbContext;
            _tagService = tagService;
        }

                
        public ICollection<SideUserDto> GetRecommendUsers(string forUsername)
        {
            List<Tag> userTags = new List<Tag>();
            if (forUsername != null)
            {
                userTags.AddRange(_userManager.FindByNameAsync(forUsername).Result.Tags.ToList());
            }

            List<ApplicationUser> recommendUsers = new List<ApplicationUser>();

            int countFill = 5 - userTags.Count;

            if (userTags.Count > 0)
            {
                for (int i = 0; i < userTags.Count; i++)
                {
                    recommendUsers.AddRange(_dbContext.Users
                        .Include(x => x.Avatar)
                        .Include(x => x.Tags)                           
                        .Where(x => x.Tags
                        .Contains(userTags.ElementAt(i))).Take(countFill + 1));
                }
            }
            else
                recommendUsers = _dbContext.Users
                    .Include(x => x.Avatar)
                    .Include(x => x.Tags)
                    .OrderByDescending(x => x.IdeaMemberRoles.Count()).Take(5).ToList();


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ApplicationUser, SideUserDto>()
                .ForMember("UserName", opt => opt.MapFrom(x => x.UserName))
                .ForMember("Guid", opt => opt.MapFrom(x => x.Id))
                .ForMember("AvatarImageName", opt => opt.MapFrom(x => x.Avatar.ImageName));
                
            });

            var mapper = new Mapper(config);

            var dtos = mapper.Map<List<ApplicationUser>, List<SideUserDto>>(recommendUsers.Take(5).ToList());

            return dtos;

        }

        public async Task<bool> InviteUserToIdea(string inviteFromUserGuid, InviteUserDto model)
        {
            var getUser = await _userManager.FindByIdAsync(inviteFromUserGuid);

            var getIdea = _dbContext.Ideas.FirstOrDefault(x => x.Title.Equals(model.InvitedToIdeaName));

            var invite = new IdeaInvitation(model.Description, getUser, getIdea, InviteTypes.Invite);

            _dbContext.IdeaInvites.Add(invite);

            return true;
        }




        #region ready
        public async Task<bool> UpdateUserSettings(string name, UserEditTagSettingsDto model)
        {
            ApplicationUser getUser = _dbContext.Users
                .Include(x => x.Tags)
                .FirstOrDefault(x => x.UserName.Equals(name));

            if (model.NewTags.Count > 0)
            {
                getUser.Tags = _tagService.CreateTagList(model.NewTags);
            }

            IdentityResult res = await _userManager.UpdateAsync(getUser);

            if (res.Succeeded)
            {
                await _signInManager.SignInAsync(getUser, false);

                return true;
            }
            else
            {
                return false;
            }


        }
        
        public async Task<bool> UpdateUserSettings(string name, UserEditGeneralSettingsDto model)
        {
            ApplicationUser getUser = _dbContext.Users
                .Include(x => x.Tags)
                .FirstOrDefault(x => x.UserName.Equals(name));

            if (!string.IsNullOrEmpty(model.NewUsername))
                getUser.UserName = model.NewUsername;

            if (!string.IsNullOrEmpty(model.NewDescription))
                getUser.Description = model.NewDescription;

            if (!string.IsNullOrEmpty(model.NewPassword) && !string.IsNullOrEmpty(model.OldPassword))
                await _userManager.ChangePasswordAsync(getUser, model.OldPassword, model.NewPassword);

            IdentityResult res = await _userManager.UpdateAsync(getUser);

            if (res.Succeeded)
            {
                await _signInManager.SignInAsync(getUser, false);

                return true;
            }
            else
            {
                return false;
            }
        }

        public UserDetailDto GetUserDetail(string guid)
        {
            ApplicationUser getUser = _dbContext.Users
                .Include(x => x.Tags)
                .Include(x => x.Followers)
                .Include(x => x.Following)
                .Include(x => x.Avatar)
                .FirstOrDefault(x => x.Id.Equals(guid));

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ApplicationUser, UserDetailDto>()
                .ForMember("Guid", opt => opt.MapFrom(x => x.Id))
                .ForMember("Name", opt => opt.MapFrom(x => x.UserName))
                .ForMember("Description", opt => opt.MapFrom(x => x.Description))
                .ForMember("Tags", opt => opt.MapFrom(x => x.Tags.Select(tag => new TagDto(tag.Name))))
                .ForMember("AvatarImageName", opt => opt.MapFrom(x => x.Avatar.ImageName))
                .ForMember("CountFollowing", opt => opt.MapFrom(x => x.Following.Count))
                .ForMember("CountFollowers", opt => opt.MapFrom(x => x.Followers.Count));
            });

            var mapper = new Mapper(config);

            return mapper.Map<ApplicationUser, UserDetailDto>(getUser);
        }

        public List<HomeIdeaDto> GetUserIdeas(string guid)
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
                .Where(x => x.Members.Any(e => e.User.Id.Equals(guid) && e.Role.Equals(IdeaMemberRoles.Author)))
                .Take(5)
                .ToList();

            var dtoIdeas = mapper.Map<List<Idea>, List<HomeIdeaDto>>(ideas);

            return dtoIdeas;
        }

        public int GetCount()
        {
            return _dbContext.Users.Count();
        }

        public int GetCount(string query)
        {
            bool isTagQuery = false;

            List<string> tagList = _dbContext.Tags.Select(x => x.Name).ToList();

            if (tagList.Contains(query)) isTagQuery = true;

            if (isTagQuery)
            {
                return _dbContext.Users
                    .Include(x => x.Tags)
                    .Where(x => x.Tags
                    .Contains(_dbContext.Tags.FirstOrDefault(x => x.Name.Equals(query))))
                    .Count();
            }
            else
                return _dbContext.Users
                    .Where(x => x.UserName.StartsWith(query))
                    .Count();
        }

        public List<UserDto> GetUsers(int page)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ApplicationUser, UserDto>()
                .ForMember("Guid", opt => opt.MapFrom(x => x.Id))
                .ForMember("UserName", opt => opt.MapFrom(x => x.UserName))
                .ForMember("UserAvatarImageName", opt => opt.MapFrom(x => x.Avatar.ImageName))
                .ForMember("Tags", opt => opt.MapFrom(x => x.Tags.Select(tag => new TagDto(tag.Name))));
            });

            var mapper = new Mapper(config);

            var users = _dbContext.Users
                .Include(x => x.Tags)
                .Include(x => x.Avatar)
                .Skip(10 * (page - 1))
                .Take(10)
                .ToList();

            return mapper.Map<List<ApplicationUser>, List<UserDto>>(users);

        }

        public List<UserDto> GetUsers(string query, int page)
        {
            bool isTagQuery = false;

            List<string> tagList = _dbContext.Tags.Select(x => x.Name).ToList();

            if (tagList.Contains(query)) isTagQuery = true;

            List<ApplicationUser> users = new List<ApplicationUser>();
            if (isTagQuery)
            {
                users = _dbContext.Users
                    .Include(x => x.Tags)
                    .Include(x => x.Avatar)
                    .Where(x => x.Tags.Contains(_dbContext.Tags.FirstOrDefault(x => x.Name.Equals(query))))
                    .Skip(10 * (page - 1))
                    .Take(10)
                    .ToList();
            }
            else
            {
                users = _dbContext.Users
                   .Include(x => x.Tags)
                   .Include(x => x.Avatar)
                   .Where(x => x.UserName.StartsWith(query))
                   .Skip(10 * (page - 1))
                   .ToList();
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ApplicationUser, UserDto>()
                .ForMember("Guid", opt => opt.MapFrom(x => x.Id))
                .ForMember("UserName", opt => opt.MapFrom(x => x.UserName))
                .ForMember("UserAvatarImageName", opt => opt.MapFrom(x => x.Avatar.ImageName))
                .ForMember("Tags", opt => opt.MapFrom(x => x.Tags.Select(tag => new TagDto(tag.Name))));
            });

            var mapper = new Mapper(config);

            return mapper.Map<List<ApplicationUser>, List<UserDto>>(users);
        }

        public int GetUserIdeasCount(string guid)
        {
            var count = _dbContext.Users
                .Include(x => x.IdeaMemberRoles)
                .Where(x => x.IdeaMemberRoles.Any(e => e.User.Id.Equals(guid) && e.Role.Equals(IdeaMemberRoles.Author)))
                .Count();

            return count;
        }

        public bool CheckSelfProfile(string routeGuid, string userName)
        {
            return _userManager.FindByIdAsync(routeGuid).Result.UserName
                .Equals(userName);
        }

        public async Task<string> GetUserGuid(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var res = await _userManager.GetUserIdAsync(user);

            return res;
        }

        public async Task<List<string>> GetIdeasToInvite(string forUsername)
        {
            var user = await _userManager.FindByNameAsync(forUsername);

            var ideas = _dbContext.Ideas
                .Include(x => x.Members)
                .Where(x => x.Members.Any(e => e.User.Equals(user) &&
                    e.Role.Equals(IdeaMemberRoles.Author)))
                .Select(x => x.Title);

            return ideas.ToList();
        }

        public List<UserParticipationDto> GetUserParticipations(string userName)
        {
            var roles = _dbContext.IdeaMemberRoles
                .Include(x => x.Idea)
                .Include(x => x.User)
                .Where(x => x.User.UserName.Equals(userName))
                .ToList();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IdeaMemberRole, UserParticipationDto>()
                .ForMember("IdeaName", opt => opt.MapFrom(x => x.Idea.Title))
                .ForMember("RoleName", opt => opt.MapFrom(x => x.Role.ToString()))
                .ForMember("IdeaGuid", opt => opt.MapFrom(x => x.Idea.Guid));
            });

            var mapper = new Mapper(config);

            var resp = mapper.Map<List<IdeaMemberRole>, List<UserParticipationDto>>(roles);

            return resp;
        }

        public async Task<bool> UserUnfollowAsync(string userGuid, string followGuid)
        {
            var getFollow = await _dbContext.Follows
                .FirstOrDefaultAsync(x => x.AuthorId.Equals(userGuid) &&
                    x.FollowingId.Equals(followGuid));

            _dbContext.Follows.Remove(getFollow);

            return true;
        }
        
        public async Task<bool> UserFollowOnAsync(string userGuid, string followGuid)
        {
            var user = await _userManager.FindByIdAsync(userGuid);

            var follow = await _userManager.FindByIdAsync(followGuid);


            if (user != null && follow != null)
            {
                var follower = new Follower(user, follow);

                await _dbContext.AddAsync(follower);

                return true;
            }
            else return false;
        }

        public async Task<bool> CheckUserFollowedAsync(string userGuid, string followGuid)
        {
            bool res = await _dbContext.Follows
                .AnyAsync(x => x.AuthorId.Equals(userGuid) && x.FollowingId.Equals(followGuid));

            return res;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public async Task<List<ChatUserDto>> GetUserChats(string userGuid)
        {
            var chats = await _dbContext.ChatMessages
                .Include(x => x.ToUser)
                .ThenInclude(x => x.Avatar)
                .Where(x => x.FromUser.Id.Equals(userGuid))
                .ToListAsync();


            var users = chats.Select(x => x.ToUserId).Distinct();

            List<ChatMessage> filter = new List<ChatMessage>();

            foreach (var user in users)
            {
                filter.Add(chats.OrderByDescending(x => x.DateCreated)
                    .FirstOrDefault(x => x.ToUserId.Equals(user)));
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChatMessage, ChatUserDto>()
                .ForMember("UserGuid", opt => opt.MapFrom(x => x.FromUserId))
                .ForMember("UserName", opt => opt.MapFrom(x => x.ToUser.UserName))
                .ForMember("AvatarImageName", opt => opt.MapFrom(x => x.ToUser.Avatar.ImageName))
                .ForMember("LastMessage", opt => opt.MapFrom(x => x.Message));
            });

            var mapper = new Mapper(config);

            var resp = mapper.Map<List<ChatMessage>, List<ChatUserDto>>(filter);

            return resp;
        }

        public async Task<List<NewChatDto>> GetNewChatUsersAsync(string guid)
        {
            List<Follower> following = await _dbContext.Follows
                .Include(x => x.Following)
                .Where(x => x.AuthorId.Equals(guid))
                .ToListAsync();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Follower, NewChatDto>()
                .ForMember("UserGuid", opt => opt.MapFrom(x => x.FollowingId))
                .ForMember("UserName", opt => opt.MapFrom(x => x.Following.UserName));
            });

            var mapper = new Mapper(config);

            var resp = mapper.Map<List<Follower>, List<NewChatDto>>(following);

            return resp;
        }

        public async Task<ChatUserDto> CreateChatWithUser(string userGuid, string chatUserGuid)
        {
            ChatMessage chatMessage = new ChatMessage(userGuid, chatUserGuid, "Hello!");

            ApplicationUser toUser = _dbContext.Users
                .Include(x => x.Avatar)
                .FirstOrDefault(x => x.Id.Equals(chatUserGuid));

            _dbContext.ChatMessages.Add(chatMessage);

            ChatUserDto resultDto = new ChatUserDto()
            {
                AvatarImageName = toUser.Avatar.ImageName,
                LastMessage = "Hello!",
                UserGuid = toUser.Id,
                UserName = toUser.UserName
            };

            return resultDto;
        }
        #endregion
    }
}
