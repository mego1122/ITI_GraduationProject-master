using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobApp.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        //[Required(ErrorMessage = "*")]
        //[StringLength(20, MinimumLength = 3, ErrorMessage = "Name must be less than 20 character")]
        //[RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Characters are not allowed.")]
        //[Display(Name = "Role Name")]
        public string Name { get; set; }
    }
}