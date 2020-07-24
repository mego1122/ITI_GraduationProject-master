using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobApp.Models
{
    public class ApplyForJob
    {
        internal string PathFile;
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime ApplyDate { get; set; }
        public int JobId { get; set; }
        public string UserId { get; set; }

        public virtual Job job { get; set; }
        public ApplicationUser user { get; set; }

    }
}