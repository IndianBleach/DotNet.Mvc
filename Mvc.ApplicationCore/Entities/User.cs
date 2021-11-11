using Mvc.ApplicationCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Entities
{
    public class UserContact
    {
        public int Id {  get; set; }
        public string Name {  get; set; }
        public string Url { get; set; }
        public ApplicationUser User { get; set; }
    }
}
