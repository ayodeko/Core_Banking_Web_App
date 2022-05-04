using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GCBA.Models
{
    public class Role
    {
        public int ID { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Role name should only contain characters with no white space"), MaxLength(40)]
        public string Name { get; set; }

        public virtual ICollection<RoleClaim> RoleClaims { get; set; }
    }

    public class RoleTypes
    {
        public string Teller = "Teller";
        public string Admin = "Admin";
        public string SuperAdmin = "SuperAdmin";

    }
}