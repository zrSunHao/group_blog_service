using Hao.GroupBlog.Persistence.Database;
using Hao.GroupBlog.Persistence.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hao.GroupBlog.Persistence.Configuration
{
    public class ColumnCfg : BaseEntityCfg<Column>
    {
        public override void EntityConfigure(EntityTypeBuilder<Column> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(64).IsRequired();

            builder.Property(x => x.Logo).HasMaxLength(64);

            builder.Property(x => x.Intro).HasMaxLength(256).IsRequired();

            builder.Property(x => x.TopicId).HasMaxLength(32).IsRequired();
        }
    }
}
