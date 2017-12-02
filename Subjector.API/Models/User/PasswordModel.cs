using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Subjector.Common;

namespace Subjector.API.Models.User
{
    public class PasswordModel : IValidatableObject
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string RepeatPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NewPassword != RepeatPassword)
            {
                yield return new ValidationResult("New Passwords don't match!.", new[] { "NewPassword" });
            }

            if (!IsStrongPassword())
            {
                yield return new ValidationResult("Password needs to contain one upper letter, one number, and to be atleast 8 digitis long!.", new[] { "NewPassword" });
            }
        }

        private bool IsStrongPassword()
        {
            if (!NewPassword.Any(char.IsUpper)) return false;
            if (!NewPassword.Any(char.IsNumber)) return false;
            if (NewPassword.Length < 8) return false;
            return true;
        }
    }
}
