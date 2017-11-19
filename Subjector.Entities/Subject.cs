using System;
using System.Collections.Generic;

namespace Subjector.Data.Entities
{
    public partial class Subject
    {
        public Subject()
        {
            SubjectActivity = new HashSet<SubjectActivity>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Archived { get; set; }

        public User CreatedByNavigation { get; set; }
        public ICollection<SubjectActivity> SubjectActivity { get; set; }
    }
}
