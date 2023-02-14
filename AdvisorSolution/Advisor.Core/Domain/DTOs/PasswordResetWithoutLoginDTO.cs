using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advisor.Core.Domain.DTOs
{
    public class PasswordResetWithoutLoginDTO
    {
        public DateTime now { get; set; } = DateTime.Now;
        [Required,EmailAddress]
        public string Email { get; set; } = null!;

    }
}
