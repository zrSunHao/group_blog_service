using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hao.GroupBlog.Persistence.Entities
{
    [Table(nameof(Favorite))]
    [Index(nameof(MemberId), IsUnique = true)]
    [Index(nameof(NoteId), IsUnique = false)]
    [Index(nameof(ColumnId), IsUnique = false)]
    public class Favorite
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string MemberId { get; set; }

        [Required]
        [MaxLength(32)]
        public string NoteId { get; set; }

        [MaxLength(32)]
        public string? ColumnId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
