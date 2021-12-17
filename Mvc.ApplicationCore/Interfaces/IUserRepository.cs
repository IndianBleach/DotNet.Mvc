using Mvc.ApplicationCore.DTOs;
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
        UserDetailDto GetUserDetail(string guid);
        ICollection<SideUserDto> GetRecommendUsers(string forUsername);
        ICollection<UserDto> GetUsersPerPage(int page);
    
    }
}
