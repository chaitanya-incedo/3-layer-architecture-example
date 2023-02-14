using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Advisor.Core.Domain.Models
{
    public class Role//role to user is 1 to many relationship like user to character 
    {
        [Key]
        public int RoleID { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(15)]
        public string RoleName { get; set; } = null!;

        [Required]
        [Column(TypeName = "Tinyint")]
        public int Active { get; set; }

        [JsonIgnore]
        List<Users> Users { get; set; } = new List<Users>();
    }
}
