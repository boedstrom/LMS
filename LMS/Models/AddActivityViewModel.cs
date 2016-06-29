using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class AddActivityViewModel
    {         
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public DateTime ModuleStart { get; set; }
        public DateTime ModuleEnd { get; set; }
        
        public IEnumerable<Activity> Activities { get; set; }
    
    }
}