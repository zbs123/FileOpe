using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class Role_User
    {
        public Role_User()
        {
            this.Users = new List<User>();
        }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<User> Users { get; set; }
    }
}
