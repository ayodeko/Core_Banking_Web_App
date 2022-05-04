using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCBA.Models
{
    public class RoleClaim
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public int RoleID { get; set; }
        public virtual Role Role { get; set; }
    }
}