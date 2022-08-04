using Hao.GroupBlog.Persistence.Attributes;
using Hao.GroupBlog.Persistence.Consts;
using Hao.GroupBlog.Persistence.Database;

namespace Hao.GroupBlog.Persistence.Entities
{
    [TablePrefix(PrefixConsts.Domain)]
    public class Domain : BaseEntity
    {
        public string Name { get; set; }
    }
}
