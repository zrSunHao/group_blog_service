using AutoMapper;
using Hao.GroupBlog.Domain.Interfaces;
using Hao.GroupBlog.Domain.Models;
using Hao.GroupBlog.Domain.Paging;
using Hao.GroupBlog.Manager.Basic;
using Hao.GroupBlog.Manager.Providers;
using Hao.GroupBlog.Persistence.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hao.GroupBlog.Manager.Implements
{
    public class MemberManager : BaseManager, IMemberManager
    {
        private readonly ILogger _logger;
        private readonly ICacheProvider _cache;

        public MemberManager(GbDbContext dbContext, 
            IMapper mapper,
            ICacheProvider cache,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ILogger<MemberManager> logger)
            : base(dbContext, mapper, configuration, httpContextAccessor)
        {
            _logger = logger;
            _cache = cache;
        }

        public async Task<ResponseResult<bool>> Register(LoginM model)
        {
            var res = new ResponseResult<bool>();
            try
            {
                
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"账号为【{model.UserName}】的用户注册失败！");
            }
            return res;
        }

        public async Task<ResponseResult<LoginRes>> Login(LoginM model)
        {
            var res = new ResponseResult<LoginRes>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"账号为【{model.UserName}】的用户登录失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> Update(MemberM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"更新账号为【{model.UserName}】的用户信息失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> ManagerResetPsd(ResetPsdM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"管理员重置账号为【{model.UserName}】的密码失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> MyselfResetPsd(ResetPsdM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"用户【{model.UserName}】重置密码失败！");
            }
            return res;
        }

        public async Task<ResponsePagingResult<MemberM>> GetList(PagingParameter<MemberFilter> parameter)
        {
            var res = new ResponsePagingResult<MemberM>();
            try
            {

                
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取用户列表失败！");
            }
            return res;
        }
    }
}
