using Mvc.ApplicationCore.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Interfaces
{
    public interface IAuthorizationService
    {
        Task LoginAsync(UserLoginDto model);
        Task LogoutAsync();
        Task RegisterAsync(UserRegisterDto model);
    }
}
