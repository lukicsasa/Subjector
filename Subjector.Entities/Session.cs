using System;

namespace Subjector.Entities
{
    public partial class Session
    {
        public int UserId { get; set; }
        public DateTime LastAction { get; set; }
        public string Token { get; set; }
        public bool Active { get; set; }

        public User User { get; set; }
    }
}
