using Hao.GroupBlog.Domain.Paging;
using System.ComponentModel.DataAnnotations;

namespace Hao.GroupBlog.Domain.Models
{
    public class DomainM
    {
        public string? Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Order { get; set; } = 1024;

        public List<TopicM> Topics { get; set; }
    }

    public class TopicM
    {
        public string? Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Logo { get; set; }

        [Required]
        public string DomainId { get; set; }

        [Required]
        public int Order { get; set; } = 1024;
    }

    public class ColumnM
    {
        public string? Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Logo { get; set; }

        [Required]
        public string Intro { get; set; }

        [Required]
        public string TopicId { get; set; }

        [Required]
        public int Order { get; set; } = 1024;
    }

    public class SequnceM
    {
        [Required]
        public string DropGroupId { get; set; }

        [Required]
        public string DragObjectId { get; set; }

        [Required]
        public List<OptionItem<int>> DropTargets { get; set; }
    }
}
