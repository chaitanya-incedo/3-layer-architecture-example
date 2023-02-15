using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Advisor.Core.Domain.Models
{
    public class AdvisorClient
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("AdvisorID")]
        public virtual Users Advisor { get; set; }

        [ForeignKey("ClientID")]
        public virtual Users Client { get; set; }

    }
}
