using Mvc.ApplicationCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Entities
{    
    public class BaseImage : BaseEntity
    {
        public string Name { get; set; }

        public BaseImage(string name)
            => Name = name;
    }
}
