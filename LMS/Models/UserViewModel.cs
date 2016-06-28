using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public enum UserType
    {
        Student,
        Teacher
    }

    public class UserViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string DefaultPassword { get; set; }
        public UserType UserType { get; set; }
        public Course Course { get; set; }
    }
}