using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs
{
    public class UserLoginDto
    {
        public string Username {  get; set; }
        public string Password {  get; set; }

        public UserLoginDto(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
