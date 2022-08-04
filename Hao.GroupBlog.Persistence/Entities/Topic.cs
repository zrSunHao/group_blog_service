using Hao.GroupBlog.Persistence.Attributes;
using Hao.GroupBlog.Persistence.Consts;
using Hao.GroupBlog.Persistence.Database;

namespace Hao.GroupBlog.Persistence.Entities
{
    [TablePrefix(PrefixConsts.Topic)]
    public class Topic : BaseEntity
    {
        public string Name { get; set; }

        public string Logo { get; set; }
    }
}
