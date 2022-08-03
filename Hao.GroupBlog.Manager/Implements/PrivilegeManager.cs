using AutoMapper;
using Hao.GroupBlog.Manager.Basic;
using Hao.GroupBlog.Manager.Providers;
using Hao.GroupBlog.Persistence.Database;
using Hao.GroupBlog.Persistence.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Hao.GroupBlog.Manager.Implements
{
    public class PrivilegeManager : BaseManager
    {
        private readonly ILogger _logger;
        private readonly ICacheProvider _cache;

        public PrivilegeManager(GbDbContext dbContext,
            IMapper mapper,
            ICacheProvider cache,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ILogger<PrivilegeManager> logger)
            : base(dbContext, mapper, configuration, httpContextAccessor)
        {
            _logger = logger;
            _cache = cache;
        }

        public async Task<UserLastLoginRecord> GetLoginRecord(string? sid)
        {
            Guid loginId = Guid.Empty;
            if (string.IsNullOrEmpty(sid)) throw new Exception("登录Id为空！");
            else
            {
                Guid.TryParse(sid, out loginId);
                if (loginId == Guid.Empty) throw new Exception("登录Id格式化失败！");
            }
            string cacheKey = loginId.ToString();
            UserLastLoginRecord? record = _cache.TryGetValue<UserLastLoginRecord>(cacheKey);
            if (record == null)
            {
                record = await _dbContext.UserLastLoginRecord.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.LoginId == loginId);
                if (record == null) throw new MyUnauthorizedException("登录信息为空！");
                else _cache.Save(cacheKey, record);
            }
            if (record.ExpiredAt <= DateTime.Now) throw new MyUnauthorizedException("登录信息已过期！");
            return record;
        }
    }
}
