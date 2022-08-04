using Hao.GroupBlog.Persistence.Database;
using Hao.GroupBlog.Persistence.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hao.GroupBlog.Persistence.Configuration
{
    public class DomainCfg : BaseEntityCfg<Domain>
    {
        public override void EntityConfigure(EntityTypeBuilder<Domain> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(64).IsRequired();

            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
