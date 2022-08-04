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
    }

    public class ColumnM
    {
        public string? Id { get; set; }

        public string Name { get; set; }

        public string Logo { get; set; }

        public string Intro { get; set; }
    }

    public class SequnceM
    {
        public string GroupKey { get; set; }

        public List<OptionItem<int>> Targets { get; set; }
    }
}
