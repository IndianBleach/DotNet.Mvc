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
        ICollection<HomeIdeaDto> GetUserIdeas(string guid);
        UserDetailDto GetUserDetail(string guid);
        ICollection<SideUserDto> GetRecommendUsers(string forUsername);
        //
        List<UserDto> GetUsers(int page);
        List<UserDto> GetUsers(string query, int page);
        int GetCount();
        int GetCount(string query);
    }
}
