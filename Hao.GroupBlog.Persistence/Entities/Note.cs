using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hao.GroupBlog.Persistence.Entities
{
    [Table(nameof(UserLastLoginRecord))]
    [Index(nameof(ContentId), IsUnique = true)]
    public class Note
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string ContentId { get; set; }

        [MaxLength(32)]
        public string? ColumnId { get; set; }

        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        [Required]
        [MaxLength(32)]
        public string Keys { get; set; }

        [Required]
        public int Hits { get; set; } = 0;

        [Required]
        public bool Opened { get; set; } = false;

        [MaxLength(32)]
        public string? ProfileId { get; set; }

        [Required]
        [MaxLength(256)]
        public string Intro { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        [MaxLength(32)]
        public string CreatedById { get; set; }

        [Required]
        public DateTime LastModifiedAt { get; set; }
    }
}
