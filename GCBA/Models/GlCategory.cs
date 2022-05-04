using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace GCBA.Models
{
    public class GlCategory
    {
        public int ID { get; set; }


        [Required(ErrorMessage = "Input GL Category Name")]
        public string Name { get; set; }
        


        public long Code { get; set; }


        [Required(ErrorMessage = "Main GL Category must be selected")]
        [Display(Name = "Main GL Category")]
        public MainGlCategory MainGlCategory { get; set; }



        [Required(ErrorMessage = "Enter Description")]
        public string Description { get; set; }
    }

    public enum MainGlCategory
    {
        Asset, Liability, Capital, Expense, Income
    }


}