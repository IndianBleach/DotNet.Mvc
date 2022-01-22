using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs
{
    public class UserRegisterDto
    {
        [Required]
        [MaxLength(20, ErrorMessage = "Maximum 20 chars")]
        [MinLength(4, ErrorMessage = "Minimum 4 chars")]
        public string Username { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "Maximum 20 chars")]
        [MinLength(4, ErrorMessage = "Minimum 4 chars")]
        public string Password { get; set; }
        [Required]
        [MaxLength(5, ErrorMessage = "Maximum 5 tags")]
        [MinLength(1, ErrorMessage = "Minimum 1 tag")]
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