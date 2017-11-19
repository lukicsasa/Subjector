using System;

namespace Subjector.Entities
{
    public partial class Result
    {
        public int StudentId { get; set; }
        public Guid SubjectActivityId { get; set; }
        public DateTime DateCreated { get; set; }
        public int Points { get; set; }
        public bool Archived { get; set; }
        public int GradedBy { get; set; }

        public User GradedByNavigation { get; set; }
        public User Student { get; set; }
        public SubjectActivity SubjectActivity { get; set; }
    }
}
