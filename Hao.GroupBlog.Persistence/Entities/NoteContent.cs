using Hao.GroupBlog.Persistence.Attributes;
using Hao.GroupBlog.Persistence.Consts;
using Hao.GroupBlog.Persistence.Database;

namespace Hao.GroupBlog.Persistence.Entities
{
    [TablePrefix(PrefixConsts.NoteContent)]
    public class NoteContent : BaseEntity
    {
        public string Content { get; set; }

        public string? Backups1 { get; set; }

        public string? Backups2 { get; set; }

        public string? Backups3 { get; set; }
    }
}
