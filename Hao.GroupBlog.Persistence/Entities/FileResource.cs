using Hao.GroupBlog.Persistence.Attributes;
using Hao.GroupBlog.Persistence.Consts;
using Hao.GroupBlog.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hao.GroupBlog.Persistence.Entities
{
    [TablePrefix(PrefixConsts.FileResource)]
    [Table(nameof(FileResource))]
    [Index(nameof(OwnId), IsUnique = false)]
    [Index(nameof(Code), IsUnique = true)]
    public class FileResource : BaseInfo
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string Code { get; set; }

        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        [Required]
        [MaxLength(32)]
        public string OwnId { get; set; }

        [Required]
        [MaxLength(16)]
        public string Category { get; set; }

        [Required]
        [MaxLength(64)]
        public string FileName { get; set; }

        [MaxLength(64)]
        public string? Type { get; set; }

        [MaxLength(16)]
        public string? Suffix { get; set; }

        [Required]
        public long Size { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [MaxLength(32)]
        public string? CreatedById { get; set; }

    }
}
