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
        public IEnumerable<Module> Modules { get; set; }
    }
}