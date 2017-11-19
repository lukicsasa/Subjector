using System;
using System.Collections.Generic;

namespace Subjector.Entities
{
    public partial class ActivityType
    {
        public ActivityType()
        {
            Activity = new HashSet<Activity>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Archived { get; set; }

        public User CreatedByNavigation { get; set; }
        public ICollection<Activity> Activity { get; set; }
    }
}
