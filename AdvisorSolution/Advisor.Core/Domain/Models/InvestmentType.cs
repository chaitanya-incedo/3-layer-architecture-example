using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advisor.Core.Domain.Models
{
    public class InvestmentType
    {
        [Key]
        public int InvestmentTypeID { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(250)]
        public string? InvestmentTypeName { get; set; }

        [Required]
        [Column(TypeName = "DateTime2")]
        public DateTime CreatedDate { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string? ModifiedBy { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime? ModifiedDate { get; set; }

        [Required]
        public int DeletedFlag { get; set; }

        [NotMapped]
        public bool DeletedFlagbool
        {
            get { return (DeletedFlag == 1); }
        }

        public List<InvestmentStrategy>? investmentStrategies { get; set; }
    }
}
