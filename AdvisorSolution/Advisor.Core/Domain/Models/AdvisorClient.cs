using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Advisor.Core.Domain.Models
{
    public class AdvisorClient
    {
        [Key]
        public int ID { get; set; }
        
        public int AdvisorId { get; set; }

        public int ClientId { get; set; }

    }
}
