using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advisor.Core.Domain
{
    public class PasswordResetDTO
    {
        public DateTime now { get; set; }=DateTime.Now;
        public string token { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
