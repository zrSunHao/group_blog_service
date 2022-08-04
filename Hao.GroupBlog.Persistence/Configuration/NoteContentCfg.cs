using Hao.GroupBlog.Persistence.Database;
using Hao.GroupBlog.Persistence.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hao.GroupBlog.Persistence.Configuration
{
    public class NoteContentCfg : BaseEntityCfg<NoteContent>
    {
        public override void EntityConfigure(EntityTypeBuilder<NoteContent> builder)
        {
            builder.Property(x => x.Content).IsRequired();

            builder.Property(x => x.Backups1);

            builder.Property(x => x.Backups2);

            builder.Property(x => x.Backups3);
        }
    }
}
