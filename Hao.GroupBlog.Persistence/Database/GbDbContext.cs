using Hao.GroupBlog.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hao.GroupBlog.Persistence.Database
{
    public class GbDbContext : DbContext
    {
        public GbDbContext(DbContextOptions<GbDbContext> options) : base(options) { }


        public DbSet<Member> Member { get; set; }
        public DbSet<UserLastLoginRecord> UserLastLoginRecord { get; set; }
        public DbSet<FileResource> FileResource { get; set; }
        public DbSet<Domain> Domain { get; set; }
        public DbSet<Topic> Topic { get; set; }
        public DbSet<Column> Column { get; set; }
        public DbSet<Note> Note { get; set; }
        public DbSet<NoteContent> NoteContent { get; set; }
        public DbSet<Favorite> Favorite { get; set; }
        public DbSet<Sequence> Sequence { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }

        /// <summary>
        /// 保存时检查并修改
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.EntityStateCheck();

            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// 自定义检查
        /// </summary>
        private void EntityStateCheck()
        {
            var addedList = ChangeTracker.Entries()
                .Where(e => (e.State == EntityState.Added) && e.Entity is BaseEntity)
                .ToList();
            if (addedList.Any())
            {
                addedList.ForEach(e => { ((BaseEntity)e.Entity).CreatedAt = DateTime.Now; });
            }

            var modifiedList = ChangeTracker.Entries()
                .Where(e => (e.State == EntityState.Modified) && e.Entity is BaseEntity)
                .ToList();
            if (modifiedList.Any())
            {
                modifiedList.ForEach(e =>
                {
                    ((BaseEntity)e.Entity).LastModifiedAt = DateTime.Now;
                    if (((BaseEntity)e.Entity).Deleted && !((BaseEntity)e.Entity).DeletedAt.HasValue)
                    {
                        ((BaseEntity)e.Entity).DeletedAt = DateTime.Now;
                    }
                });
            }
        }
    }
}
