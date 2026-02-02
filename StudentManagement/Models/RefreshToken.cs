using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        [ForeignKey(nameof(UserId))]
        public IdentityUser? User { get; set; }

        public string Token { get; set; } = string.Empty;

        public string JwtId { get; set; } = string.Empty;

        public bool IsUsed { get; set; }

        public bool IsRevoked { get; set; }

        public DateTime AddedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
