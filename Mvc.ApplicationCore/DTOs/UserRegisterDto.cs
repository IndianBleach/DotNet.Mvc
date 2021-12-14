using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs
{
    public class UserRegisterDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public List<string> Tags { get; set; }

        public UserRegisterDto()
        {
        }

        public UserRegisterDto(string username, string password, List<string> tags)
        {
            Username = username;
            Password = password;
            Tags = tags;
        }
    }
}