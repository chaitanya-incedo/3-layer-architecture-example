using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advisor.Core.Domain
{
    public class AdvisorDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Key]
        public int AdvisroId { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required,Phone]
        public string Phone { get; set; } = string.Empty;
        [Required]
        public string Company { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public string State { get; set; } = string.Empty;
        [Required,MinLength(6)]
        public string Password { get; set; } = string.Empty;
        [Required,Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
