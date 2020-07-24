using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobApp.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "you should enter valid name")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Name must be less than 20 character")]
        [Display(Name = "Category Name")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
         ErrorMessage = "Characters are not allowed.")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Category Description")]
        public string CategoryDescription { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }

    }
}