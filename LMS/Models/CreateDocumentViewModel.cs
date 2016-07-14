using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class CreateDocumentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ParentClass { get; set; }
        public DocParent ParentType { get; set; }
        public Document Document { get; set; }
    }
}