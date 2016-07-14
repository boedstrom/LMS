using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class ShowUsersViewModel
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public DateTime CourseStart { get; set; }
        public DateTime CourseEnd { get; set; }

        public IEnumerable<ApplicationUser> Users { get; set; }
    }
}