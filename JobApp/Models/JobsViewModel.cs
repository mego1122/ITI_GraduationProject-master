using System.Linq;
using JobApp.Models;
using System.Collections.Generic;

namespace JobApp.Models
{
    public class JobsViewModel
    {
        public string JobTitle { get; set; }
        public IEnumerable<ApplyForJob> Items { get; set; }
    }
}