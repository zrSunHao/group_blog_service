using Hao.GroupBlog.Common.Enums;

namespace Hao.GroupBlog.Domain.Models
{
    public class MemberM
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public RoleType Role { get; set; }

        public bool Limited { get; set; }

        public string? Remark { get; set; }

        public DateTime? LastLoginAt { get; set; }
    }

    public class MemberFilter
    {
        public string? UserName { get; set; }

        public RoleType? Role { get; set; }

        public bool? Limited { get; set; }

        public DateTime? StartAt { get; set; }

        public DateTime? EndAt { get; set; }
    }

    public class LoginM
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class LoginRes
    {
        public string UserName { get; set; }

        public RoleType Role { get; set; }

        public string Key { get; set; }
    }

    public class ResetPsdM
    {
        public string UserName { get; set; }

        public string NewPsd { get; set; }

        public string OldPsd { get; set; }
    }
}
