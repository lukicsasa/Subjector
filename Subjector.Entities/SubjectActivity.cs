using System;
using System.Collections.Generic;

namespace Subjector.Data.Entities
{
    public partial class SubjectActivity
    {
        public SubjectActivity()
        {
            Result = new HashSet<Result>();
        }

        public int SubjectId { get; set; }
        public int ActivityId { get; set; }
        public decimal Weight { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Archived { get; set; }
        public Guid Id { get; set; }

        public Activity Activity { get; set; }
        public User CreatedByNavigation { get; set; }
        public Subject Subject { get; set; }
        public ICollection<Result> Result { get; set; }
    }
}
