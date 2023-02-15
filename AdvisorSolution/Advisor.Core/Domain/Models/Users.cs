
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Advisor.Core.Domain.Models
{
    public class Users
    {
        [Key]
        public int UserID { get; set; }

        public Role Role { get; set; } = null!;
        public int RoleID { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(200)]
        public string? Address { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(30)]
        public string? City { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string? State { get; set; }

        [Required]
        [Phone]
        [Column(TypeName = "VARCHAR")]
        [StringLength(40)]
        public string Phone { get; set; } = null!;

        [Required]
        [EmailAddress]
        [Column(TypeName = "VARCHAR")]
        [StringLength(30)]
        public string Email { get; set; } = null!;

        [Column(TypeName = "VARCHAR")]
        [StringLength(6)]
        public string? AdvisorID { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(6)]
        public string? AgentID { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(6)]
        public string? ClientID { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string LastName { get; set; } = null!;

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string FirstName { get; set; } = null!;

        [Column(TypeName = "VARCHAR")]
        [StringLength(150)]
        public string? Company { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(100)]
        public string SortName { get; set; } = null!;

        [Required]
        [Column(TypeName = "Tinyint")]
        public int Active { get; set; }

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

        public byte[] PasswordHash { get; set; }=new byte[0];

        public byte[] PasswordSalt { get; set; }=new byte[0];

        public string? VerificationToken { get; set; }

        public DateTime? VerifiedAt { get; set; }

        public string? PasswordResetToken { get; set; }

        public DateTime? ResetTokenExpires { get; set; }

        public List<InvestorInfo>? investorInfos { get; set; }

        public virtual List<AdvisorClient> AdvisorsList { get; set; }
        public virtual List<AdvisorClient> ClientList { get; set; }
    }
}
