using AutoMapper;
using Hao.GroupBlog.Common.Enums;
using Hao.GroupBlog.Domain.Consts;
using Hao.GroupBlog.Persistence.Database;
using Hao.GroupBlog.Persistence.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Hao.GroupBlog.Manager.Basic
{
    public class BaseManager
    {
        protected readonly GbDbContext _dbContext;
        protected readonly IMapper _mapper;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IConfiguration _configuration;

        public BaseManager(GbDbContext dbContext,
            IMapper mapper,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string GetConfiguration(string key) => _configuration[key];

        /// <summary>
        /// 获取当前Http请求头信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected KeyValuePair<string, StringValues> GetHeader(string key) => _httpContextAccessor.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == key);

        /// <summary>
        /// 生成文件加载url
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected string BuilderFileUrl(string? fileName, string key = "")
        {
            if (string.IsNullOrEmpty(fileName)) return "";
            var baseUrl = FileResourceUrl;
            if (string.IsNullOrEmpty(key)) key = CurrentLoginId.ToString();
            if (fileName.Contains(baseUrl)) return fileName;
            return $"{baseUrl}?name={fileName}&key={key}";
        }

        /// <summary>
        /// 机器码
        /// </summary>
        protected string MachineCode => GetConfiguration(CfgConsts.PLATFORM_MACHINE_CODE);

        /// <summary>
        /// 资源地址
        /// </summary>
        protected string FileResourceUrl => GetConfiguration(CfgConsts.FILE_RESOURCE_BASE_URL);

        protected string? RemoteIpAddress => _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString();

        /// <summary>
        /// 用户登录记录
        /// </summary>
        public UserLastLoginRecord LoginRecord => GetLoginRecord();

        /// <summary>
        /// 当前用户Id
        /// </summary>
        public string CurrentUserId => LoginRecord.MemberId;

        /// <summary>
        /// 当前用户登录Id
        /// </summary>
        public Guid CurrentLoginId => LoginRecord.LoginId;

        public RoleType CurrentUserRole => this.LoginRecord.Role;

        public string CurrentUserName => this.GetCurrentUserName();


        private UserLastLoginRecord? _lastLoginRecord;
        private UserLastLoginRecord GetLoginRecord()
        {
            if (_lastLoginRecord != null) return _lastLoginRecord;
            _httpContextAccessor.HttpContext.Items.TryGetValue(nameof(UserLastLoginRecord), out object? obj);
            if (obj == null) throw new MyCustomException("未查询到登录信息！");
            else if (obj is UserLastLoginRecord) _lastLoginRecord = obj as UserLastLoginRecord;
            else throw new MyCustomException("未查询到登录信息！");
            if (_lastLoginRecord == null) throw new MyCustomException("未查询到登录信息！");
            else return _lastLoginRecord;
        }

        private string? _currentUserName;
        private string GetCurrentUserName()
        {
            if (!string.IsNullOrWhiteSpace(_currentUserName)) return _currentUserName;
            var member = _dbContext.Member.FirstOrDefault(x => x.Id == CurrentUserId);
            var userName = member == null ? "" : member.UserName;
            _currentUserName = userName;
            return userName;
        }
    }
}
