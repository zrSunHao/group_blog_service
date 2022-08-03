using Hao.GroupBlog.Common.Enums;

namespace Hao.GroupBlog.Domain.Models
{
    public class ResourceM
    {
        public string Code { get; set; }

        public string FileName { get; set; }

        public FileCategory Category { get; set; }

        public string? Type { get; set; }

        public long Size { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class ResourceFilter
    {
        public string? FileName { get; set; }

        public string? type { get; set; }

        public FileCategory? Category { get; set; }

        public DateTime? StartAt { get; set; }

        public DateTime? EndAt { get; set; }
    }

    public class FileM
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string OwnId { get; set; }

        public FileCategory Category { get; set; }

        public string FileName { get; set; }

        public string? Type { get; set; }

        public string? Suffix { get; set; }

        public long Size { get; set; }
    }
}
