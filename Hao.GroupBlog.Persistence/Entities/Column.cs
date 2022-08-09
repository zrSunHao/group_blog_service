using Hao.GroupBlog.Persistence.Attributes;
using Hao.GroupBlog.Persistence.Consts;
using Hao.GroupBlog.Persistence.Database;

namespace Hao.GroupBlog.Persistence.Entities
{
    [TablePrefix(PrefixConsts.Column)]
    public class Column : BaseEntity
    {
        public string Name { get; set; }

        public string? Logo { get; set; }

        public string Intro { get; set; }

        public string TopicId { get; set; }
    }
}
