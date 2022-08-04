using Hao.GroupBlog.Persistence.Database;
using Hao.GroupBlog.Persistence.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hao.GroupBlog.Persistence.Configuration
{
    public class TopicCfg : BaseEntityCfg<Topic>
    {
        public override void EntityConfigure(EntityTypeBuilder<Topic> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(64).IsRequired();

            builder.Property(x => x.Logo).HasMaxLength(64).IsRequired();

            builder.Property(x => x.DomainId).HasMaxLength(32).IsRequired();
        }
    }
}
