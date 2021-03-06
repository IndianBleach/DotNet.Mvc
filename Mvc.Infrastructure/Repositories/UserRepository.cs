using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Mvc.ApplicationCore.ChatHub;
using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.DTOs.Chat;
using Mvc.ApplicationCore.DTOs.Idea;
using Mvc.ApplicationCore.DTOs.JsonResult;
using Mvc.ApplicationCore.DTOs.News;
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
using System.Security.Claims;
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
        private IHubContext<ChatHub> _chatContext;


        public UserRepository(ITagService tagService, ApplicationContext dbContext, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
             IHubContext<ChatHub> chat)
        {
            _chatContext = chat;
            _signInManager = signInManager;
            _userManager = userManager;
            _dbContext = dbContext;
            _tagService = tagService;
        }

                
        public ICollection<SideUserDto> GetRecommendUsers(string forUsername)
        {
            List<Tag> userTags = new List<Tag>();

            var getUser = _userManager.FindByNameAsync(forUsername).Result;

            if (getUser != null)
            {
                userTags.AddRange(getUser.Tags.ToList());
            }

            List<ApplicationUser> recommendUsers = new List<ApplicationUser>();

            int countFill = 4 - userTags.Count;

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
                    .OrderByDescending(x => x.IdeaMemberRoles.Count()).Take(4).ToList();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ApplicationUser, SideUserDto>()
                .ForMember("UserName", opt => opt.MapFrom(x => x.UserName))
                .ForMember("Guid", opt => opt.MapFrom(x => x.Id))
                .ForMember("AvatarImageName", opt => opt.MapFrom(x => x.Avatar.ImageName));
                
            });

            var mapper = new Mapper(config);

            var dtos = mapper.Map<List<ApplicationUser>, List<SideUserDto>>(recommendUsers.Take(4).ToList());

            return dtos;
        }

        public async Task<bool> InviteUserToIdea(string inviteFromUserGuid, InviteUserDto model)
        {
            var fromUser = await _userManager.FindByIdAsync(inviteFromUserGuid);

            var InvitedUser = await _userManager.FindByIdAsync(model.InvitedUserGuid);

            var getIdea = _dbContext.Ideas.FirstOrDefault(x => x.Title.Equals(model.InvitedToIdeaName));

            var invite = new IdeaInvitation(model.Description, fromUser, getIdea, InviteTypes.Invite, InvitedUser);

            _dbContext.IdeaInvites.Add(invite);

            return true;
        }
       
        public async Task<bool> UpdateUserSettings(string name, UserEditTagSettingsDto model)
        {
            ApplicationUser getUser = _dbContext.Users
                .Include(x => x.Tags)
                .FirstOrDefault(x => x.UserName.Equals(name));

            if (getUser != null)
                if (model.NewTags != null && model.NewTags.Count > 0)
                {
                    getUser.Tags = _tagService.CreateTagList(model.NewTags);
                    IdentityResult res = await _userManager.UpdateAsync(getUser);
                    if (res.Succeeded)
                    {
                        await _signInManager.SignInAsync(getUser, false);

                        return true;
                    }
                }
            
            return false;
        }
        
        public async Task<bool> UpdateUserSettings(IEnumerable<ClaimsIdentity> identities, string name, UserEditGeneralSettingsDto model)
        {
            ApplicationUser getUser = _dbContext.Users
                .Include(x => x.Avatar)
                .Include(x => x.Tags)
                .FirstOrDefault(x => x.UserName.Equals(name));

            if (getUser != null)
            {
                if (model.NewAvatar != null)
                {
                    using (FileStream str = new FileStream($"wwwroot/media/userAvatars/" + model.NewAvatar.FileName, FileMode.Create))
                    {
                        await model.NewAvatar.CopyToAsync(str);
                        str.Close();
                    }

                    if (getUser.Avatar.ImageName != "def_user_avatar.jpg")
                    {
                        File.Delete($"wwwroot/media/userAvatars/{getUser.Avatar.ImageName}");
                    }

                    getUser.Avatar = new UserAvatarImage(model.NewAvatar.FileName);

                    //change avatar claim
                    var userIdentity = identities.FirstOrDefault(x => x.Name == name);
                    var getClaim = userIdentity.FindFirst("AvatarImageName");
                    Claim newClaim = new Claim("AvatarImageName", model.NewAvatar.FileName);

                    await _userManager.ReplaceClaimAsync(getUser, getClaim, newClaim);
                }

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
                else return false;
            }

            return false;            
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
                .ForMember("CountFollowing", opt => opt.MapFrom(x => x.Followers.Count))
                .ForMember("CountSubscribers", opt => opt.MapFrom(x => x.Following.Count))
                .ForMember("DateCreated", opt => opt.MapFrom(x => GeneratePublishDate(x.AccountDateCreated)));
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

        private string GetNormalizeRoleName(IdeaMemberRoles role)
        {
            return role switch
            {
                IdeaMemberRoles.Author => "Author",
                IdeaMemberRoles.Modder => "Modder",
                IdeaMemberRoles.Default => "Member",
                _ => "",
            };
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
                .ForMember("RoleName", opt => opt.MapFrom(x => GetNormalizeRoleName(x.Role)))
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
        
        public async Task<NewChatDtoExtended> GetNewChatUsersAsync(string guid)
        {
            List<ApplicationUser> getAuthorChatUsers = _dbContext.Chats
                .Include(x => x.Users)
                .ThenInclude(x => x.User)
                .Where(x => x.Users.Any(x => x.UserId.Equals(guid)))
                .ToList()
                .Select(x => x.Users.FirstOrDefault(x => x.UserId != guid))
                .Select(x => x.User)
                .ToList();                           

            List<ApplicationUser> following = await _dbContext.Follows
                .Include(x => x.Following)
                .ThenInclude(x => x.Avatar)
                .Where(x => x.AuthorId.Equals(guid))
                .Select(x => x.Following)
                .ToListAsync();

            var except = following.Except(getAuthorChatUsers).ToList();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ApplicationUser, NewChatDto>()
                .ForMember("UserGuid", opt => opt.MapFrom(x => x.Id))
                .ForMember("UserName", opt => opt.MapFrom(x => x.UserName))
                .ForMember("AvatarImageName", opt => opt.MapFrom(x => x.Avatar.ImageName));
            });

            var mapper = new Mapper(config);

            var users = mapper.Map<List<ApplicationUser>, List<NewChatDto>>(except);

            NewChatDtoExtended result = new()
            {
                NewChatUsers = users,
                AuthorGuid = guid
            };

            return result;
        }        

        public async Task<List<IdeaToInviteDto>> GetUserIdeasToInvite(string guid)
        {
            var res = await _dbContext.Ideas
                .Include(x => x.Members)
                .Where(x => x.Members.Any(x => x.UserId.Equals(guid)
                    && x.Role.Equals(IdeaMemberRoles.Author)))
                .ToListAsync();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Idea, IdeaToInviteDto>()
                .ForMember("Guid", opt => opt.MapFrom(x => x.Guid))
                .ForMember("Name", opt => opt.MapFrom(x => x.Title));
            });

            var mapper = new Mapper(config);

            var resp = mapper.Map<List<Idea>, List<IdeaToInviteDto>>(res);

            return resp;
        }

        public async Task<CreatedChatDto> CreateChat(string authorGuid, string message, string toUserGuid)
        {
            ApplicationUser messageAuthor = _dbContext.Users
                .Include(x => x.Avatar)
                .FirstOrDefault(x => x.Id.Equals(authorGuid));

            ApplicationUser toUser = _dbContext.Users
                .Include(x => x.Avatar)
                .FirstOrDefault(x => x.Id.Equals(toUserGuid));

            bool isExist = _dbContext.Chats
                .Include(x => x.Users)
                .Any(x => x.Users.All(x => x.Id.ToString() == authorGuid &&
                    x.Id.ToString() == toUserGuid));

            if (!isExist)
            {
                Chat createChat = new Chat();

                ChatMessage createMessage = new ChatMessage(messageAuthor, message, createChat);

                ChatUser chatUser = new ChatUser(messageAuthor, createChat);
                ChatUser chatUserSec = new ChatUser(toUser, createChat);

                createChat.Messages.Add(createMessage);
                createChat.Users.Add(chatUser);
                createChat.Users.Add(chatUserSec);

                _dbContext.Chats.Add(createChat);

                _dbContext.SaveChanges();

                CreatedChatDto dto = new();
                dto.Guid = createChat.Guid.ToString();
                dto.Messages = new List<MessageDetailDto>()
                {
                    new MessageDetailDto()
                    {
                        AuthorName = messageAuthor.UserName,
                        AvatarImageName = messageAuthor.Avatar.ImageName,
                        DateCreated = createMessage.DateCreated.ToShortDateString(),
                        IsAuthorMessage = true,
                        Message = message
                    }
                };
                dto.UserGuid = toUserGuid;
                dto.UserName = toUser.UserName;
                dto.AvatarImageName = toUser.Avatar.ImageName;
                dto.AuthorGuid = authorGuid;

                return dto;
            }

            return null;
        }

        public async Task<List<ChatUserDto>> GetUserChats(string userGuid)
        {
            var exc = _dbContext.Chats
                .Include(x => x.Messages)
                .Include(x => x.Users)
                .ThenInclude(x => x.User)
                .ThenInclude(x => x.Avatar)
                .Where(x => x.Users
                    .Any(y => y.UserId == userGuid))
                .ToList();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Chat, ChatUserDto>()
                .ForMember("MemberGuid", opt => opt.MapFrom(x => x.Users.FirstOrDefault(e => e.User.Id != userGuid).User.Id))
                .ForMember("ChatGuid", opt => opt.MapFrom(x => x.Guid))
                .ForMember("UserName", opt => opt.MapFrom(x => x.Users
                    .Where(x => x.User.Id != userGuid).SingleOrDefault().User.UserName))
                .ForMember("AvatarImageName", opt => opt.MapFrom(x => x.Users
                    .Where(x => x.UserId != userGuid).First().User.Avatar.ImageName))
                .ForMember("LastMessage", opt => opt.MapFrom(x => x.Messages
                    .OrderBy(x => x.DateCreated)
                    .First()
                    .Text));
            });

            var mapper = new Mapper(config);

            var resp = mapper.Map<List<Chat>, List<ChatUserDto>>(exc);

            return resp;

        }

        public async Task<ChatDetailDto> GetChatDetail(string chatGuid, string currentUsername)
        {
            ApplicationUser currentUser = await _userManager.FindByNameAsync(currentUsername);

            var getChat = _dbContext.Chats
                .Include(x => x.Messages)
                .ThenInclude(x => x.Author)
                .ThenInclude(x => x.Avatar)
                .FirstOrDefault(x => x.Guid.ToString() == chatGuid);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChatMessage, MessageDetailDto>()
                .ForMember("AuthorName", opt => opt.MapFrom(x => x.Author.UserName))
                .ForMember("AvatarImageName", opt => opt.MapFrom(x => x.Author.Avatar.ImageName))
                .ForMember("Message", opt => opt.MapFrom(x => x.Text))
                .ForMember("DateCreated", opt => opt.MapFrom(x => x.DateCreated.ToShortDateString()))
                .ForMember("IsAuthorMessage", opt => opt.MapFrom(x => x.Author.UserName.Equals(currentUsername)));
            });

            var mapper = new Mapper(config);
            var resp = mapper.Map<List<ChatMessage>, List<MessageDetailDto>>(getChat.Messages.ToList());

            ChatDetailDto chatDto = new ChatDetailDto()
            {
                AuthorInfo = new ChatAuthorInfoDto()
                {
                    AvatarImageName = currentUser.Avatar.ImageName,
                    Guid = currentUser.Id,
                    UserName = currentUser.UserName
                },
                Messages = resp
            };

            return chatDto;
        }

        public async Task<List<MessageDetailDto>> GetChatMessages(string chatGuid, string currentUsername)
        {
            var getChat = _dbContext.Chats
                .Include(x => x.Messages)
                .ThenInclude(x => x.Author)
                .ThenInclude(x => x.Avatar)
                .FirstOrDefault(x => x.Guid.ToString() == (chatGuid));            

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChatMessage, MessageDetailDto>()
                .ForMember("AuthorName", opt => opt.MapFrom(x => x.Author.UserName))
                .ForMember("AvatarImageName", opt => opt.MapFrom(x => x.Author.Avatar.ImageName))
                .ForMember("Message", opt => opt.MapFrom(x => x.Text))
                .ForMember("DateCreated", opt => opt.MapFrom(x => x.DateCreated.ToShortDateString()))
                .ForMember("IsAuthorMessage", opt => opt.MapFrom(x => x.Author.UserName.Equals(currentUsername)));                
            });

            var mapper = new Mapper(config);

            var resp = mapper.Map<List<ChatMessage>, List<MessageDetailDto>>(getChat.Messages.ToList());

            return resp;

        }

        public async Task<bool> SendChatMessage(string chatGuid, string message, string authorGuid, string toUserGuid)
        {
            //ApplicationUser getUser = await _userManager.FindByIdAsync(authorGuid);            

            var getChat = _dbContext.Chats
                .Include(x => x.Messages)
                .FirstOrDefault(x => x.Guid.ToString().Equals(chatGuid));

            ChatMessage createMessage = new ChatMessage(authorGuid, message);

            getChat.Messages.Add(createMessage);

            await _dbContext.SaveChangesAsync();

            RecieveMessageDto recieveMessage = new RecieveMessageDto()
            {
                Text = createMessage.Text,                
            };

            await _chatContext.Clients.Group(chatGuid).SendAsync("RecieveMessage", recieveMessage);

            return true;
        }

        public async Task<List<InviteDetailDto>> GetUserInvites(string userGuid)
        {
            ApplicationUser getAuthor = await _userManager.FindByIdAsync(userGuid);

            var res = _dbContext.IdeaInvites
                .Include(x => x.Author)
                .ThenInclude(x => x.Avatar)
                .Include(x => x.InvitedUser)
                .ThenInclude(x => x.Avatar)
                .Include(x => x.RelateIdea)
                .Where(x => x.InvitedUser == getAuthor)
                .ToList();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IdeaInvitation, InviteDetailDto>()
                .ForMember("InviteGuid", opt => opt.MapFrom(x => x.Guid))
                .ForMember("FromUserGuid", opt => opt.MapFrom(x => x.Author.Id))
                .ForMember("FromUserAvatar", opt => opt.MapFrom(x => x.Author.Avatar.ImageName))
                .ForMember("FromUserName", opt => opt.MapFrom(x => x.Author.UserName))
                .ForMember("IdeaGuid", opt => opt.MapFrom(x => x.RelateIdea.Guid))
                .ForMember("IdeaName", opt => opt.MapFrom(x => x.RelateIdea.Title))
                .ForMember("Description", opt => opt.MapFrom(x => x.Content));
            });

            var mapper = new Mapper(config);

            var resp = mapper.Map<List<IdeaInvitation>, List<InviteDetailDto>>(res);

            return resp;
        }

        public async Task<string> UserAcceptInvite(string inviteGuid)
        {
            var getInvite = await _dbContext.IdeaInvites
                .Include(x => x.RelateIdea)
                .Include(x => x.InvitedUser)
                .FirstOrDefaultAsync(x => x.Guid.ToString() == inviteGuid);

            var createRole = new IdeaMemberRole(
                    IdeaMemberRoles.Default,
                    getInvite.InvitedUserId,
                    getInvite.RelateIdeaId);

            _dbContext.IdeaMemberRoles.Add(createRole);

            _dbContext.IdeaInvites.Remove(getInvite);

            return getInvite.Guid.ToString();
        }

        public async Task<string> UserRejectInvite(string inviteGuid)
        {
            var getInvite = await _dbContext.IdeaInvites
                .FirstOrDefaultAsync(x => x.Guid.ToString() == inviteGuid);
            
            _dbContext.IdeaInvites.Remove(getInvite);

            return getInvite.Guid.ToString();
        }

        public async Task<bool> CreateIdeaJoinRequest(string description, string ideaGuid, string userGuid)
        {
            var getIdea = await _dbContext.Ideas
                .FirstOrDefaultAsync(x => x.Guid.ToString() == ideaGuid);

            if (getIdea != null)
            {
                var data = new IdeaInvitation(description, userGuid, getIdea.Id, InviteTypes.JoinRequest, null);

                _dbContext.IdeaInvites.Add(data);
                    
                return true;
            }

            return false;
        }

        public async Task<bool> SendFastInviteAsync(string ideaGuid, string userGuid, string authorGuid)
        {
            var getIdea = await _dbContext.Ideas
                .FirstOrDefaultAsync(x => x.Guid.ToString() == ideaGuid);

            if (getIdea != null)
            {
                var data = new IdeaInvitation("Join my idea!", authorGuid, getIdea.Id, InviteTypes.Invite, userGuid);

                _dbContext.IdeaInvites.Add(data);

                return true;
            }

            return false;
        }

        public async Task<List<UserFollowingDto>> GetUserSubscribersAsync(string userGuid)
        {
            var subs = await _dbContext.Follows
                .Include(x => x.Author)
                .ThenInclude(x => x.Avatar)
                .Where(x => x.FollowingId.Equals(userGuid))
                .ToListAsync();

            //var followers = author.Followers;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Follower, UserFollowingDto>()
                .ForMember("Name", opt => opt.MapFrom(x => x.Author.UserName))
                .ForMember("Guid", opt => opt.MapFrom(x => x.Author.Id))
                .ForMember("AvatarImageName", opt => opt.MapFrom(x => x.Author.Avatar.ImageName));                
            });

            var mapper = new Mapper(config);

            var resp = mapper.Map<List<Follower>, List<UserFollowingDto>>(subs);

            return resp;
        }

        public async Task<List<UserFollowingDto>> GetUserFollowingsAsync(string userGuid)
        {
            var subs = await _dbContext.Follows
                .Include(x => x.Following)
                .ThenInclude(x => x.Avatar)
                .Where(x => x.AuthorId.Equals(userGuid))
                .ToListAsync();

            //var followers = author.Followers;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Follower, UserFollowingDto>()
                .ForMember("Name", opt => opt.MapFrom(x => x.Following.UserName))
                .ForMember("Guid", opt => opt.MapFrom(x => x.Following.Id))
                .ForMember("AvatarImageName", opt => opt.MapFrom(x => x.Following.Avatar.ImageName));
            });

            var mapper = new Mapper(config);

            var resp = mapper.Map<List<Follower>, List<UserFollowingDto>>(subs);

            return resp;
        }

        public async Task<List<NewsDto>> GetNewsAsync()
        {
            List<NewsDto> news = new();

            List<NewsDto> users = await _dbContext.Users
                .Include(x => x.Avatar)
                .OrderByDescending(x => x.AccountDateCreated)
                .Take(3)
                .Select(x => new NewsDto()
                { 
                    IdeaGuid = null,
                    UserAvatarName = x.Avatar.ImageName,
                    UserGuid = x.Id,
                    Date = x.AccountDateCreated,
                    UserName = x.UserName,
                    IdeaName = null
                })
                .ToListAsync();

            news.AddRange(users);

            List<NewsDto> ideas = await _dbContext.Ideas
                .Include(x => x.Members)
                .ThenInclude(x => x.User)
                .ThenInclude(x => x.Avatar)
                .OrderByDescending(x => x.DateCreated)
                .Take(3)
                .Select(x => new NewsDto()
                {
                    IdeaGuid = x.Guid.ToString(),
                    UserAvatarName = x.Members.FirstOrDefault(x => x.Role
                        .Equals(IdeaMemberRoles.Author)).User.Avatar.ImageName,
                    UserGuid = x.Members.FirstOrDefault(x => x.Role
                        .Equals(IdeaMemberRoles.Author)).User.Id,
                    Date = x.DateCreated,
                    UserName = x.Members.FirstOrDefault(x => x.Role
                        .Equals(IdeaMemberRoles.Author)).User.UserName,
                    IdeaName = x.Title
                })
                .ToListAsync();

            news.AddRange(ideas);

            var res = news.OrderByDescending(x => x.Date)
                .Take(4)
                .ToList();

            return res;
        }
    }
}
