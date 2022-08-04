using Hao.GroupBlog.Domain.Paging;

namespace Hao.GroupBlog.Domain.Models
{
    public class DomainM
    {
        public string? Id { get; set; }

        public string Name { get; set; }
    }

    public class TopicM
    {
        public string? Id { get; set; }

        public string Name { get; set; }

        public string Logo { get; set; }

        public string DomainId { get; set; }
    }

    public class ColumnM
    {
        public string? Id { get; set; }

        public string Name { get; set; }

        public string Logo { get; set; }

        public string Intro { get; set; }

        public string TopicId { get; set; }
    }

    public class SequnceM
    {
        public string? DragGroupKey { get; set; }

        public string DropGroupKey { get; set; }

        public List<OptionItem<int>> DropTargets { get; set; }
    }
}
