using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Subjector.Common.Models
{
    public class UserModel : IValidatableObject
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
        [RegularExpression("^[0-9]{8}$", ErrorMessage = "RefCode needs to be in yyyyxxxx format")]
        public string RefCode { get; set; }
        public int Role { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(RefCode)
                && (Role == (int)Common.Role.PendingStudent || Role == (int)Common.Role.Student))
            {
                yield return new ValidationResult("Ref code is required!.", new[] { "RefCode" });
            }
        }
    }
}
