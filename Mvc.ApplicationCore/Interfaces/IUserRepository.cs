using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.DTOs.Chat;
using Mvc.ApplicationCore.DTOs.Idea;
using Mvc.ApplicationCore.DTOs.JsonResult;
using Mvc.ApplicationCore.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Interfaces
{
    public interface IUserRepository
    {
        Task<ChatDetailDto> GetChatDetail(string chatGuid, string currentUsername);
        Task<bool> SendChatMessage(string chatGuid, string message, string authorGuid);
        //
        ICollection<SideUserDto> GetRecommendUsers(string forUsername);
        Task<List<MessageDetailDto>> GetChatMessages(string chatGuid, string currentUserName);
        Task<List<IdeaToInviteDto>> GetUserIdeasToInvite(string guid);
        Task<CreatedChatDto> CreateChat(string authorGuid, string message, string toUserGuid);
        Task<List<ChatUserDto>> GetUserChats(string userGuid);
        Task<List<NewChatDto>> GetNewChatUsersAsync(string guid);
        //
        void Save();
        Task<bool> CheckUserFollowedAsync(string userGuid, string followGuid);
        Task<bool> UserFollowOnAsync(string userGuid, string followGuid);
        Task<bool> UserUnfollowAsync(string userGuid, string unfollowGuid);
        //
        List<UserParticipationDto> GetUserParticipations(string userGuid);
        //
        Task<List<string>> GetIdeasToInvite(string forUsername);
        Task<bool> InviteUserToIdea(string invitedFromUserGuid, InviteUserDto model);
        Task<string> GetUserGuid(string userName);
        Task<bool> UpdateUserSettings(string guid, UserEditTagSettingsDto model);
        Task<bool> UpdateUserSettings(string guid, UserEditGeneralSettingsDto model);
        bool CheckSelfProfile(string routeGuid, string userName);
        List<HomeIdeaDto> GetUserIdeas(string guid);
        UserDetailDto GetUserDetail(string guid);
        List<UserDto> GetUsers(int page);
        List<UserDto> GetUsers(string query, int page);
        int GetCount();
        int GetCount(string query);
        int GetUserIdeasCount(string guid);
    }
}
