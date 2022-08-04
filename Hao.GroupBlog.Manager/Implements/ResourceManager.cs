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

        public async Task<ResponseResult<bool>> Save(FileM model)
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

        public async Task<ResponseResult<FileM>> GetByCode(string code)
        {
            var res = new ResponseResult<FileM>();
            try
            {
                var entity = await _dbContext.FileResource
                    .FirstOrDefaultAsync(x => x.Code == code);
                if (entity == null) throw new MyCustomException("未查询到文件信息");
                res.Data = _mapper.Map<FileM>(entity);
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取编码为【{code}】的文件失败！");
            }
            return res;
        }

        public async Task<ResponsePagingResult<ResourceM>> GetList(PagingParameter<ResourceFilter> parameter)
        {
            var res = new ResponsePagingResult<ResourceM>();
            try
            {


            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取资源列表失败！");
            }
            return res;
        }

        public string GetNewCode()
        {
            var entity = new FileResource();
            return entity.GetId(MachineCode);
        }

        public string BuilderFileUrl(string? fileName)
        {
            return base.BuilderFileUrl(fileName);
        }
    }
}
