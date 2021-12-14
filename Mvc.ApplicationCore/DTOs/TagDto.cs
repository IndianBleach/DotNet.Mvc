using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.DTOs
{
    public class TagDto
    {
        public string Name { get; set; }

        public TagDto(string name)
        {
            Name = name;
        }
    }
}
