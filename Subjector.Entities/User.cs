using System;
using System.Collections.Generic;

namespace Subjector.Entities
{
    public partial class User
    {
        public User()
        {
            Activity = new HashSet<Activity>();
            ActivityType = new HashSet<ActivityType>();
            Cert = new HashSet<Cert>();
            ResultGradedByNavigation = new HashSet<Result>();
            ResultStudent = new HashSet<Result>();
            Subject = new HashSet<Subject>();
            SubjectActivity = new HashSet<SubjectActivity>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateCreated { get; set; }
        public int Role { get; set; }
        public string RefCode { get; set; }
        public bool Archived { get; set; }

        public Session Session { get; set; }
        public ICollection<Activity> Activity { get; set; }
        public ICollection<ActivityType> ActivityType { get; set; }
        public ICollection<Cert> Cert { get; set; }
        public ICollection<Result> ResultGradedByNavigation { get; set; }
        public ICollection<Result> ResultStudent { get; set; }
        public ICollection<Subject> Subject { get; set; }
        public ICollection<SubjectActivity> SubjectActivity { get; set; }
    }
}
