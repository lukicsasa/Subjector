using System;
using System.ComponentModel.DataAnnotations;

namespace Subjector.Common.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public DateTime DateCreated { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [RegularExpression("^[0-9]{8}$", ErrorMessage = "RefCode needs to be in yyyyxxxx format")]
        public string RefCode { get; set; }
        public int Role { get; set; }
    }
}
