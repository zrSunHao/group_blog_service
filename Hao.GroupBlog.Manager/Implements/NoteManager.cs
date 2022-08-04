using AutoMapper;
using Hao.GroupBlog.Domain.Interfaces;
using Hao.GroupBlog.Domain.Models;
using Hao.GroupBlog.Domain.Paging;
using Hao.GroupBlog.Manager.Basic;
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
    public class NoteManager : BaseManager, INoteManager
    {
        private readonly ILogger _logger;

        public NoteManager(GbDbContext dbContext,
            IMapper mapper,
            ILogger<NoteManager> logger,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
            : base(dbContext, mapper, configuration, httpContextAccessor)
        {
            _logger = logger;
        }

        public async Task<bool> IsOpen(string id)
        {
            var res = false;
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"获取笔记【{id}】分享状态失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> Add(NoteM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"添加笔记【{model.Name}】信息失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> Update(NoteM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"添加笔记【{model.Name}】信息失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> Delete(string id)
        {
            var res = new ResponseResult<bool>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"删除笔记【{id}】失败！");
            }
            return res;
        }

        public async Task<ResponsePagingResult<NoteM>> GetList(PagingParameter<string> parameter)
        {
            var res = new ResponsePagingResult<NoteM>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取笔记列表失败！搜索条件为【{parameter.Filter}】");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> ToColumn(string id, string columnId)
        {
            var res = new ResponseResult<bool>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"移动笔记【{id}】到专栏【{columnId}】失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> Favorite(string id, string? columnId)
        {
            var res = new ResponseResult<bool>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"收藏笔记【{id}】到专栏【{columnId}】失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> CancelFavorite(string id)
        {
            var res = new ResponseResult<bool>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"取消收藏笔记【{id}】失败！");
            }
            return res;
        }

        public async Task<ResponseResult<NoteContentM>> GetContent(string id)
        {
            var res = new ResponseResult<NoteContentM>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取笔记【{id}】内容失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> SaveContent(NoteContentM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"保存笔记【{model.Id}】的内容失败！");
            }
            return res;
        }

        public async Task<ResponseResult<NoteContentM>> GetOpenedContent(string id)
        {
            var res = new ResponseResult<NoteContentM>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取分享笔记【{id}】内容失败！");
            }
            return res;
        }
    }
}
