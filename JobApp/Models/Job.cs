using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobApp.Models
{
    public class Job
    {
        public int Id { get; set; }

        [Display(Name = "Job Name")]
        [Required(ErrorMessage = "you should enter valid name")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Name must be less than 20 character")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
         ErrorMessage = "Characters are not allowed.")]
        public string JobTitle { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Job Description")]
        [AllowHtml]
        public string JobContent { get; set; }


        [Display(Name = "Job Image")]
        public string JobImage { get; set; }


        [Display(Name = "Job Type")]
        [ForeignKey("Category")]

        public int CategoryId { get; set; }
        [ForeignKey("User")]

        public string UserID { get; set; }

        public Category Category { get; set; }


        public ApplicationUser User { get; set; }

    }
}