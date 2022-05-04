using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCBA.Models
{
    public class TellersTill
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int GlAccounID { get; set; }
        public virtual GlAccount GlAccount { get; set; }
    }
}