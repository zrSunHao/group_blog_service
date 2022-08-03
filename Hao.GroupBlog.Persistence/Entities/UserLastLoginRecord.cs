using Hao.GroupBlog.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hao.GroupBlog.Persistence.Entities
{
    [Table(nameof(UserLastLoginRecord))]
    [Index(nameof(LoginId), IsUnique = true)]
    [Index(nameof(MemberId), IsUnique = false)]
    public class UserLastLoginRecord
    {
        [Key]
        public long Id { get; set; }

        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid LoginId { get; set; }

        [Required]
        [MaxLength(32)]
        public string MemberId { get; set; }

        [Required]
        [MaxLength(32)]
        public RoleType Role { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime ExpiredAt { get; set; }
    }
}
