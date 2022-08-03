using Hao.GroupBlog.Persistence.Database;
using Hao.GroupBlog.Persistence.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hao.GroupBlog.Persistence.Configuration
{
    public class MemberCfg : BaseEntityCfg<Member>
    {
        public override void EntityConfigure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(32).IsRequired();

            builder.Property(x => x.Role).IsRequired();

            builder.Property(x => x.Password).IsRequired();

            builder.Property(x => x.PasswordSalt).IsRequired();

            builder.Property(x => x.Limited).IsRequired();

            builder.Property(x => x.Remark).HasMaxLength(512);


            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
