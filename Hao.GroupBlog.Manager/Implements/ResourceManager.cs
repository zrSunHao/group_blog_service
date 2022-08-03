using AutoMapper;
using Hao.GroupBlog.Domain.Interfaces;
using Hao.GroupBlog.Domain.Models;
using Hao.GroupBlog.Domain.Paging;
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
    public class ResourceManager : BaseManager, IResourceManager
    {
        private readonly string CODE_PREFIX = "FR";
        private string _codeTime = "";
        private int _codeIndex = 1;
        private Random _codeRandom = new Random();
        private object _codeLock = new object();
        private readonly ILogger _logger;
        private readonly ICacheProvider _cache;
        public ResourceManager(GbDbContext dbContext,
            IMapper mapper,
            ICacheProvider cache,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ResourceManager> logger)
            : base(dbContext, mapper, configuration, httpContextAccessor)
        {
            _logger = logger;
            _cache = cache;
        }

        public async Task<ResponseResult<bool>> Save(ResourceM model)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var entity = _mapper.Map<FileResource>(model);
                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"保存名称为【{model.Name}】- 【{model.FileName}】的文件失败！");
            }
            return res;
        }

        public async Task<ResponseResult<ResourceM>> GetByCode(string code)
        {
            var res = new ResponseResult<ResourceM>();
            try
            {
                var entity = await _dbContext.FileResource
                    .FirstOrDefaultAsync(x => x.Code == code);
                if (entity == null) throw new MyCustomException("未查询到文件信息");
                res.Data = _mapper.Map<ResourceM>(entity);
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取编码为【{code}】的文件失败！");
            }
            return res;
        }


        public string GetNewCode()
        {
            lock (_codeLock)
            {
                string time = DateTime.Now.ToString("yyMMddHHmmss");
                if (time != _codeTime) { _codeTime = time; _codeIndex = 1; }
                else _codeIndex++;
            }
            string rand = _codeRandom.Next(100, 999).ToString();
            // 前缀2位、时间12位、顺序码5位、随机码3位、机器码4位
            return $"{CODE_PREFIX}{_codeTime}{string.Format("{0:D5}", _codeIndex)}{rand}_{MachineCode}";
        }

        public string BuilderFileUrl(string? fileName)
        {
            return base.BuilderFileUrl(fileName);
        }
    }
}
