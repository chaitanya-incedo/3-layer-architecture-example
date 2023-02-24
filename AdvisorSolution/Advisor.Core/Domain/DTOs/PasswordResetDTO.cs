using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advisor.Core.Domain
{
    public class PasswordResetDTO
    {
        public string email { get; set; } = null;
        public string token { get; set; } = null!;
        public string Password { get; set; } = null!;

    }
}
