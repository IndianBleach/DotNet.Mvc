using Mvc.ApplicationCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Entities
{
    public class Follower
    {
        public int Id { get; set; }
        public ApplicationUser Author { get; set; }
        public ApplicationUser Following { get; set; }
    }
}
