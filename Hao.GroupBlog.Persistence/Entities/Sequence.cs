using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hao.GroupBlog.Persistence.Entities
{
    [Table(nameof(Sequence))]
    [Index(nameof(GroupKey), IsUnique = true)]
    public class Sequence
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string GroupKey { get; set; }

        [Required]
        [MaxLength(32)]
        public string TrgetId { get; set; }

        [Required]
        public int Order { get; set; } = 0;
    }
}
