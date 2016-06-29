using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public enum ActivityType
    {
        Lecture,
        ELearning
    }

    public class Activity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public ActivityType Type { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        public DateTime? Deadline { get; set; }

        public virtual Module Module { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
    }
}