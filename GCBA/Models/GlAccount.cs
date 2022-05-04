using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace GCBA.Models
{
    public class GlAccount
    {
        public int ID { get; set; }


        [Required(ErrorMessage = "Input the GL Account Name")]
        [RegularExpression(@"[a-zA-Z ]+$", ErrorMessage = "Must Contain only Characters")]
        [Display(Name = "Account Name")]
        public string AccountName { get; set; }



        
        [Display (Name = "GL Account Code")]
        public long Code { get; set; }



        [Display (Name = "Account Balance")]
        [DataType(DataType.Currency)]
        public decimal AccountBalance { get; set; }



        [Required(ErrorMessage = "Select a Branch")]
        public int BranchID { get; set; }
        public Branch Branch { get; set; }



        
        public int GlCategoryID { get; set; }
        public virtual GlCategory GlCategory { get; set; }
        
    }
}