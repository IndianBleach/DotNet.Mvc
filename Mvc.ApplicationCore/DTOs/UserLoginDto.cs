using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs
{
    public class UserLoginDto
    {
        [Required]
        [MaxLength(20, ErrorMessage = "Maximum 20 chars")]
        [MinLength(4, ErrorMessage = "Minimum 4 chars")]
        public string Username {  get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "Maximum 20 chars")]
        [MinLength(4, ErrorMessage = "Minimum 4 chars")]
        [DataType(DataType.Password)]
        public string Password {  get; set; }

        public UserLoginDto()
        {
        }

        public UserLoginDto(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
