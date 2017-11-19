namespace Subjector.Entities
{
    public partial class Cert
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CertNumber { get; set; }
        public bool? Active { get; set; }

        public User User { get; set; }
    }
}
