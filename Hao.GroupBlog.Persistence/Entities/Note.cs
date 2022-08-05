using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hao.GroupBlog.Persistence.Entities
{
    [Table(nameof(Note))]
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

        [MaxLength(64)]
        public string? ProfileName { get; set; }

        [Required]
        [MaxLength(256)]
        public string Intro { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        [MaxLength(32)]
        public string CreatedById { get; set; }

        public DateTime? LastModifiedAt { get; set; }

        public string? LastModifiedById { get; set; }

        [Required]
        public bool Deleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }

        /// </summary>
        public string? DeletedById { get; set; }
    }
}
