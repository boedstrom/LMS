using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class AddModuleViewModel
    {
        public int Id { get; set; }
        public int CourseId  { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public DateTime CourseStart { get; set; }
        public DateTime CourseEnd { get; set; }

        public IEnumerable<Module> Modules { get; set; }
    }
}