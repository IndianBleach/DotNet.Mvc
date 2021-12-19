using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.DTOs.Idea;
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
        ICollection<SideUserDto> GetRecommendUsers(string forUsername);
        //
        Task<string> GetUserGuid(string userName);
        Task UpdateUserSettings(string guid, UserEditTagSettingsDto model);
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
