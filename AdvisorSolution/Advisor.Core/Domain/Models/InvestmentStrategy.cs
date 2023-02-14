using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advisor.Core.Domain.Models
{
    public class InvestmentStrategy
    {
        [Key]
        public int InvestmentStrategyID { get; set; }

        public InvestorInfo InvestorInfo { get; set; } = null!;
        public int InvestorInfoID { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(200)]
        public string StrategyName { get; set; } = null!;

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(6)]
        public string AccountID { get; set; } = null!;

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(6)]
        public string ModelAPLID { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(17,5)")]
        public decimal InvestmentAmount { get; set; }

        public InvestmentType InvestmentTypes { get; set; } = null!;
        public int InvestmentTypeID { get; set; }

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
    }
}
