using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advisor.Core.Domain.DTOs
{
    public class ClientInfoDto
    {
        public string LastName { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string? Company { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string Phone { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? ClientID { get; set; }
    }
}
