using Hao.GroupBlog.Common.Enums;
using Hao.GroupBlog.Persistence.Attributes;
using Hao.GroupBlog.Persistence.Consts;
using Hao.GroupBlog.Persistence.Database;

namespace Hao.GroupBlog.Persistence.Entities
{
    [TablePrefix(PrefixConsts.Member)]
    public class Member : BaseEntity
    {
        public string UserName { get; set; }

        public RoleType Role { get; set; }

        public byte[] Password { get; set; }

        public byte[] PasswordSalt { get; set; }

        public bool Limited { get; set; } = false;

        public string? Remark { get; set; }
    }
}
